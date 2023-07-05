using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueWindow;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private float textSpeed;

    [SerializeField] private GameObject prevButton;

    public string[] sentences;
    public int index;

    void Update()
    {
        ShowPreviousButton();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        index = 0;
        int length = 0;

        dialogueWindow.SetActive(true);
        Array.Clear(sentences, 0, sentences.Length);

        //dialogue.sentences.CopyTo(sentences, 0);

        foreach (string sentence in dialogue.sentences)
        {
            sentences.SetValue(sentence, length);
            length++;
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Length == 0)//|| index == sentences.Length
        {
            dialogueWindow.SetActive(false);
            return;
        }

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentences[index]));

        index++;
    }

    public void DisplayPreviousSentence()
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentences[index-1]));

        index--;
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

    private void ShowPreviousButton()
    {
        if(index > 0)
        {
            prevButton.SetActive(true);
        }
        else
        {
            prevButton.SetActive(false); 
        }
    }
}