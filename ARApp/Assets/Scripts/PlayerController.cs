using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    public static PlayerController instance;

    public int healthPoints = 100;
    bool isAlive;
    public bool IsAlive {
        get { return isAlive; }
    }

    private void Awake() {
        instance = this;
        isAlive = true;
    }

    private void Update() {
        if (healthPoints <= 0) {
            isAlive = false;
        }
    }

    public void TakeDamageFromEnemy() {
        healthPoints -= 20;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("EnemyShot")) {
            gameManager.AngryMob();
        }
    }
}