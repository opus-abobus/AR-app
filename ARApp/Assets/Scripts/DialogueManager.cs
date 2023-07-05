using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] private float textSpeed;
    [SerializeField] private Text dialogueText;
    private GameObject dialogueWindow;

    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
        dialogueWindow = GetComponent<GameObject>();
    }

    private void StartDialogue(Dialogue dialogue)
    {
        dialogueWindow.gameObject.SetActive(true);
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            dialogueWindow.gameObject.SetActive(false);
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }
}