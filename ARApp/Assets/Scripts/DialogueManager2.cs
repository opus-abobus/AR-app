using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager2 : MonoBehaviour
{
    [SerializeField] float typeSpeed = 1;

    [SerializeField] GameObject dialogueWindow;
    [SerializeField] TextMeshProUGUI dialogueText;

    [SerializeField] Button positiveAnswerButton;
    [SerializeField] Button negativeAnswerButton;
    [SerializeField] TextMeshProUGUI positiveButtonText;
    [SerializeField] TextMeshProUGUI negativeButtonText;

    [SerializeField] EnemyController enemyController;
    [SerializeField] PlayerController playerController;

    [HideInInspector] public enum AnswerType {
        Positive,
        Negative,
        Alternative,
        Neutral
    }

    //<summary>
    // A: means positive answer type, B - negative, C - alternative, D - Neutral
    //<summary>
   enum SentenceID {
        start, A, B, C, AA, AB, AAA1, AAA2, AAA3, AAA
    };
    string[] sentences;
    string[] positiveButtonSentences, negativeButtonSentences;

    // ��� �������� ��������� � ���������� �������...
    void InitSentences() {
        int idCount = Enum.GetValues(typeof(SentenceID)).Length;
        sentences = new string[idCount];
        positiveButtonSentences = new string[idCount];
        negativeButtonSentences = new string[idCount];

        sentences[(int) SentenceID.start] = "����� ���������� � ��� ����������, ������.";
        positiveButtonSentences[(int)SentenceID.start] = "������";
        negativeButtonSentences[(int)SentenceID.start] = "����� � ������";

        sentences[(int) SentenceID.A] = "� ���� ���� ���-��� ��� ����.\n���� �� � ����� �������, �� ��� ����� ������ �����, ��� �������?";
        positiveButtonSentences[(int)SentenceID.A] = "����, ��� �� �����������?";
        negativeButtonSentences[(int)SentenceID.A] = "� ���� ��� �� ��� �������";

        sentences[(int) SentenceID.B] = "�� � ���� � �����!";

        sentences[(int) SentenceID.C] = "��, ������ ��! ����� ��� �������� �� �����������?";
        positiveButtonSentences[(int)SentenceID.C] = "��, ������, � �� �������";

        sentences[(int) SentenceID.AA] = "�������� �� ��������� ������� ��������?";
        positiveButtonSentences[(int)SentenceID.AA] = "��";
        negativeButtonSentences[(int)SentenceID.AA] = "� ���� ��� �� ��� �������";

        sentences[(int) SentenceID.AB] = "��� �, ��� ������.";
        
        sentences[(int) SentenceID.AAA1] = "��� ����������� ��� ���������?";
        positiveButtonSentences[(int)SentenceID.AAA1] = positiveButtonSentences[(int)SentenceID.AAA2] = positiveButtonSentences[(int)SentenceID.AAA3] = "��";
        negativeButtonSentences[(int)SentenceID.AAA1] = negativeButtonSentences[(int)SentenceID.AAA2] = negativeButtonSentences[(int)SentenceID.AAA3] = "���";

        sentences[(int) SentenceID.AAA2] = "�������� ��, ��� �� ��� ��������� � ��� ���������?";

        sentences[(int) SentenceID.AAA3] = "����������� ���� ��������� ����� ��������?";

        sentences[(int) SentenceID.AAA] = "������������, � ���� ��������������� � ���� ��� �������� ��� ��������� ������ �� ����� ������� ���������.\n����������� ����!";
    }

    private void Awake() {
        InitSentences();
    }

    [HideInInspector] SentenceID currentSentenceID;
    public void StartDialogue() {
        dialogueWindow.SetActive(true);

        currentSentenceID = SentenceID.start;
        StartCoroutine(TypeSentence(sentences[(int) currentSentenceID]));

        UpdateButtonText(currentSentenceID);
    }

    void UpdateButtonText(SentenceID sentenceID)
    {
        positiveButtonText.text = positiveButtonSentences[(int) sentenceID];
        negativeButtonText.text = negativeButtonSentences[(int) sentenceID];
    }

    public void EndDialogue(EndDialogType endDialogType) {

        positiveAnswerButton.onClick.RemoveAllListeners();
        negativeAnswerButton.onClick.RemoveAllListeners();

        if (endDialogType == EndDialogType.startFight) {
            print("fight started");

            dialogueText.text = "";
            dialogueWindow.SetActive(false);

            StartCoroutine(Fighting());

            return;
        }
        else {
            dialogueWindow.SetActive(false);
        }

        StopAllCoroutines();
        print("end: " + endDialogType);
    }

    IEnumerator Fighting() {
        while (playerController.IsAlive && enemyController.isAlive) {
            yield return null;
        }

        if (!playerController.IsAlive) {
            endType = EndDialogType.deadPlayer;
        }
        else {
            endType = EndDialogType.deadMob;
        }
        StartCoroutine(SmoothEnd(1));
        //EndDialogue(endType);
    }

    [HideInInspector] public AnswerType lastAnswerType;

    public void GiveAnswer(AnswerType answerType) {
        StopCoroutine(TypeSentence(sentences[(int) lastAnswerType]));

        lastAnswerType = answerType;

        if (enemyController.isAngry) {
            //isHit = true;
            return;
        }

        DetermineNextSentenceID();

        StartCoroutine(TypeSentence(sentences[(int) currentSentenceID]));

        UpdateButtonText(currentSentenceID);

        Debug();
    }

    void Debug() {
        print("lastAnswerType: " + lastAnswerType);
    }

    bool isHit = false;
    public void HitTheMob() {

        if (enemyController.isAngry) {
            //return;
        }

        if (isHit) { 
            enemyController.isAngry = true; 
            endType = EndDialogType.startFight; 
            EndDialogue(endType);
            return;
        }

        GiveAnswer(AnswerType.Alternative);

        /*lastAnswerType = AnswerType.Alternative;

        if (!isHit ) {
            DetermineNextSentenceID();
            StartCoroutine(TypeSentence(sentences[(int) currentSentenceID]));
            isHit = true;
        }
        else {
            enemyController.isAngry = true;
        }*/
    }

    void DetermineNextSentenceID() {
        switch (currentSentenceID) {

            // FROM: start
            case SentenceID.start: {
                    switch (lastAnswerType) {
                        case AnswerType.Positive: { currentSentenceID = SentenceID.A; break; }
                        case AnswerType.Negative: { currentSentenceID = SentenceID.B; break; }
                        case AnswerType.Alternative: { currentSentenceID = SentenceID.C; break; }
                    }
                    break;
                }
            
            // FROM: A
            case SentenceID.A: {
                    switch (lastAnswerType) {
                        case AnswerType.Positive: { currentSentenceID = SentenceID.AA; break; }
                        case AnswerType.Negative: { currentSentenceID = SentenceID.AB; endType = EndDialogType.normal; StartCoroutine(SmoothEnd(1)); break; }
                    }
                    break;
                }

            // FROM: B
            case SentenceID.B: {
                    // normal end
                    endType = EndDialogType.normal;
                    EndDialogue(endType);
                    break;
                }

            // FROM: C
            case SentenceID.C: {
                    switch (lastAnswerType) {
                        case AnswerType.Positive: { currentSentenceID = SentenceID.A; break; }

                        // fight starts
                        case AnswerType.Alternative: { 

                                if (endType != EndDialogType.startFight) {
                                    endType = EndDialogType.startFight; 
                                    EndDialogue(endType);
                                }
                                    
                                break;
                            }
                    }
                    break;
                }

            // FROM: AA
            case SentenceID.AA: {
                    switch (lastAnswerType) {
                        case AnswerType.Positive: { currentSentenceID = SentenceID.AAA1; break; }
                        case AnswerType.Negative: { 
                                currentSentenceID = SentenceID.AB;
                                endType = EndDialogType.normal;
                                StartCoroutine(SmoothEnd(1));
                                break; 
                            }
                    }
                    break;
                }

            // FROM: AB
            case SentenceID.AB: {
                    // normal end
                    endType = EndDialogType.normal;
                    EndDialogue(endType);
                    break;
                }

            // FROM: AAA
            case SentenceID.AAA1: {
                    lastAnswerType = AnswerType.Neutral;

                    currentSentenceID = SentenceID.AAA2;

                    // store the answer

                    break;
                }

            // FROM: AAA2
            case SentenceID.AAA2: {
                    lastAnswerType = AnswerType.Neutral;

                    currentSentenceID = SentenceID.AAA3;

                    // store the answer

                    break;
                }

            // FROM: AAA3
            case SentenceID.AAA3: {
                    lastAnswerType = AnswerType.Neutral;

                    currentSentenceID = SentenceID.AAA;

                    // store the answer

                    break;
                }

            // FROM: AAA3
            case SentenceID.AAA: {
                    // rewarded end
                    endType = EndDialogType.rewarded;
                    EndDialogue(endType);
                    break;
                }
        }
    }

    [HideInInspector] public enum EndDialogType {
        normal, deadMob, deadPlayer, rewarded, startFight
    }
    EndDialogType endType;

    IEnumerator TypeSentence(string sentence) {

        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return null;
        }

    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.K)) {
            StartDialogue();
        }
    }

    private void OnEnable() {
        positiveAnswerButton.onClick.AddListener(() => GiveAnswer(AnswerType.Positive));
        negativeAnswerButton.onClick.AddListener(() => GiveAnswer(AnswerType.Negative));
    }

    private void OnDisable() {
        positiveAnswerButton.onClick.RemoveAllListeners();
        negativeAnswerButton.onClick.RemoveAllListeners();
    }

    IEnumerator SmoothEnd(float delay) {
        positiveAnswerButton.gameObject.SetActive(false);
        negativeAnswerButton.gameObject.SetActive(false);

        yield return new WaitForSeconds(delay);

        EndDialogue(endType);
    }
}
