using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private Canvas mask;
    [SerializeField] private GameObject dialogueWindow;

    private EnemyController enemyController;

    public void MaskHide()
    {
        mask.gameObject.SetActive(false);
    }

    public void MaskShow()
    {
        mask.gameObject.SetActive(true);
    }

    /*public void DialogueHide()
    {
        dialogueWindow.SetActive(false);
    }

    public void DialogueShow()
    {
        dialogueWindow.SetActive(true);
    }*/
}
