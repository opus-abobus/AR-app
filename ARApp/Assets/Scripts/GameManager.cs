using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject mobPrefab;
    [SerializeField] GameObject mobInstance;
    EnemyController enemyController;
    [SerializeField] GameObject targetObject;
    [SerializeField] VuforiaBehaviour vuforiaBehaviour;


    bool isMobSpawned;
    private void Awake()
    {
        instance = this;
        isMobSpawned = false;
        enemyController = mobInstance.GetComponent<EnemyController>();
    }

    public void SpawnMob()
    {
        if (!isMobSpawned)
        {
            isMobSpawned = true;
            Instantiate(mobPrefab, targetObject.transform.position, mobPrefab.transform.rotation, targetObject.transform);
            //vuforiaBehaviour.enabled = false;
        }
    }

    public void AngryMob() {
        enemyController.AngryMob();
    }


}
