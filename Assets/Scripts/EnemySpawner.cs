using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public Transform spawnPoint;
    public EnemyController enemyPrefab;
    public bool isActive;
    public float spawnDelay;
    public int totalToSpawn = 0;

    private float lastSpawn = Mathf.NegativeInfinity;
    private GameObject player;
    private int spawnedCount = 0;
    

    void Start()
    {
        // todo: a better way to do this
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (spawnedCount >= totalToSpawn)
        {
            GameEvents.current.SpawnerEmpty(gameObject);
            isActive = false;
        }

        if (isActive && ((lastSpawn + spawnDelay) < Time.time))
        {
            var newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.LookRotation(spawnPoint.forward));
            newEnemy.SetWalkTarget(player.transform);
            lastSpawn = Time.time;
            spawnedCount += 1;
        }   
    }
}
