using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Animation anim;
    [SerializeField] private AudioClip[] audioClip;
    [SerializeField] private int health = 10;
    [SerializeField] private Slider healthBar;
    private AudioSource audioSource;
   
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        healthBar.value = health;

        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit Hit;
            if(Physics.Raycast(ray, out Hit))
            {
                if(Hit.transform.name == "StoneMonster")
                {
                    TakeDamage();
                }
            }
        }
    }

    private void TakeDamage()
    {
        health--;

        if(health > 0)
        {
            anim.CrossFade("Anim_Damage");
            audioSource.clip = audioClip[0];
            audioSource.Play();
        }
        else
        {
            anim.CrossFade("Anim_Death");
            audioSource.clip = audioClip[1];
            audioSource.Play();
            healthBar.gameObject.SetActive(false);
        }
    }
}