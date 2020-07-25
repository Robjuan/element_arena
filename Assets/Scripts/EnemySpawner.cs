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

    private List<EnemyController> spawnedEnemies = new List<EnemyController>();
    

    void Start()
    {
        // todo: a better way to do this
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // if the spawner is exhausted
        if (spawnedCount >= totalToSpawn)
        {
            // stop spawning
            isActive = false;

            var allDead = true;
            // check that every enemy we spawned is dead
            foreach(EnemyController enemy in spawnedEnemies)
            {
                // if any is alive, then not all are dead
                if (enemy)
                {
                    allDead = false;
                    break;
                }
            }

            // if we're exhausted and every enemy is dead, spawner is empty.
            // todo: consider separating this.
            if (allDead)
            {
                GameEvents.current.SpawnerEmpty(gameObject);
            }
        }

        if (isActive && ((lastSpawn + spawnDelay) < Time.time))
        {
            var newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.LookRotation(spawnPoint.forward));
            newEnemy.SetWalkTarget(player.transform);
            lastSpawn = Time.time;
            spawnedCount += 1;
            spawnedEnemies.Add(newEnemy);
        }   
    }
}
