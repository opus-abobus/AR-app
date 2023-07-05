using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private Canvas mask;

    public void MaskHide()
    {
        mask.gameObject.SetActive(false);
    }

    public void MaskShow()
    {
        mask.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("EnemyShot")) {
            GameManager.instance.AngryMob();
        }
    }
}
