using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] GameManager gameManager;
    [SerializeField] Image imageWithDamage;

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
        StopAllCoroutines();
        StartCoroutine(DamageEffect(1));
    }

    IEnumerator DamageEffect(float duration) {
        float t = 0;

        var color = imageWithDamage.color;
        imageWithDamage.color = new Color(color.r, color.g, color.b, 1);
        color = imageWithDamage.color;

        while (t < duration) {

            color.a = Mathf.Lerp(color.a, 0, t);
            imageWithDamage.color = color;

            t += Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("EnemyShot")) {
            gameManager.AngryMob();
        }
    }
}