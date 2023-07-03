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
    [SerializeField] private GameObject enemyPrefab;
    private AudioSource audioSource;
    private bool inputEnable = true;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        healthBar.value = health;

        if (inputEnable)
        {
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit Hit;
                if (Physics.Raycast(ray, out Hit))
                {
                    if (Hit.transform.name == "StoneMonster")
                    {
                        TakeDamage();
                    }
                }
            }
        }

        if (anim.IsPlaying("Anim_Damage") == false && anim.IsPlaying("Anim_Idle") == false && inputEnable == true)
        {
            anim.CrossFade("Anim_Attack");
        }
    }

    private void TakeDamage()
    {
        health--;

        if (health > 0)
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

            inputEnable = false;
            StartCoroutine(Respawner());
        }
    }

    IEnumerator Respawner()
    {
        yield return new WaitForSeconds(3f);
        anim.CrossFade("Anim_Idle");
        health = 10;
        healthBar.gameObject.SetActive(true);

        inputEnable = true;
    }
}