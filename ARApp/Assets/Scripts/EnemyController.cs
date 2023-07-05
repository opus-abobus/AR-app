using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] GameObject boundaries;
    [SerializeField] GameObject enemyShotPrefab;
    [SerializeField] Transform shotSpawnPoint;
    [SerializeField] float hitCooldown;
    [SerializeField, Range(1, 100)] float shotSpeed = 10f;
    [SerializeField] private Animation anim;
    [SerializeField] private AudioClip[] audioClip;
    [SerializeField] private int health = 10;
    [SerializeField] private Slider healthBar;
    private AudioSource audioSource;
    private bool inputEnable = true;
    private bool isAlive = true;
    
    void Start()
    {
        isMoving = false;
        audioSource = GetComponent<AudioSource>();
        InitBoundaries();
    }

    float minZ, maxZ, minX, maxX, minY, maxY;
    void InitBoundaries()
    {
        minZ = boundaries.GetNamedChild("-ZB").transform.position.z;
        maxZ = boundaries.GetNamedChild("ZB").transform.position.z;
        minX = boundaries.GetNamedChild("-XB").transform.position.x;
        maxX = boundaries.GetNamedChild("XB").transform.position.x;
        minY = boundaries.GetNamedChild("YB").transform.position.y;
        maxY = boundaries.GetNamedChild("-YB").transform.position.y;
        //Destroy(boundaries);
        print("-Z: " + minZ); print("Z: " +  maxZ); print("-X " + minX); print("X " + maxX); print("-Y " + minY); print("Y " +  maxY);
    }
    public void AngryMob() {
        if (PlayerController.instance.IsAlive)
            StartCoroutine(Shooting());
    }

    IEnumerator Shooting() {
        var shot = Instantiate(enemyShotPrefab, shotSpawnPoint.position, transform.rotation);
        Vector3 a = transform.position;
        Vector3 b = camera.transform.position;
        float t = 0;

        while (t < 1) {
            t += 0.01f * shotSpeed;
            shot.transform.position = Vector3.Lerp(a, b, t);
            yield return new WaitForEndOfFrame();
        }

        Destroy(shot);
        PlayerController.instance.TakeDamageFromEnemy();
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
                        int rnd = Random.Range(0, 5);
                        if (rnd != 0) {
                            TakeDamage();
                        }
                        
                    }
                }
            }
        }

        if (anim.IsPlaying("Anim_Damage"))
        {
            inputEnable = false;
            StartCoroutine(Antispam());
        }

        Attack();
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
            isAlive = false;
            StartCoroutine(Respawner());
        }
    }

    bool isMoving;
    public void StartMove()
    {
        if (!isMoving)
        {
            StartCoroutine(Moving());
            isMoving = true;
        }
        
    }

    IEnumerator Moving()
    {
        float t = 0;
        while (true)
        {
            while (t < 1)
            {
                Vector3 a = transform.position;
                Vector3 b = Vector3.zero;

                int rnd = Random.Range(0, 6);
                if (rnd == 0) b = new Vector3(minX, a.y, a.z);
                else if (rnd == 1) b = new Vector3(maxX, a.y, a.z);
                else if (rnd == 2) b = new Vector3(a.x, minY, a.z);
                else if (rnd == 3) b = new Vector3(a.x, maxY, a.z);
                //else if (rnd == 4) b = new Vector3(a.x, a.y, maxZ);
                //else if (rnd == 5) b = new Vector3(a.x, a.y, minZ);

                t += 0.01f;
                transform.position = Vector3.Lerp(a, b, t);
                yield return new WaitForEndOfFrame();
            }
            t = 0;
            
            yield return new WaitForSeconds(2);
        }
    }

    IEnumerator Respawner()
    {
        yield return new WaitForSeconds(3f);
        anim.CrossFade("Anim_Idle");
        health = 10;
        healthBar.gameObject.SetActive(true);

        inputEnable = true;
        isAlive = true;
    }

    private void Attack()
    {
        if (anim.IsPlaying("Anim_Damage") == false && anim.IsPlaying("Anim_Idle") == false && isAlive == true)
        {
            anim.CrossFade("Anim_Attack");
        }
    }

    IEnumerator Antispam()
    {
        yield return new WaitForSeconds(1f);
        inputEnable = true;
    }
}