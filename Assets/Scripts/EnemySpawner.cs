using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public Transform spawnPoint;
    public EnemyController enemyPrefab;
    public bool isActive;
    public float spawnDelay;

    private float lastSpawn = Mathf.NegativeInfinity;
    private GameObject player;

    void Start()
    {
        // todo: a better way to do this
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (isActive && ((lastSpawn + spawnDelay) < Time.time))
        {
            var newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.LookRotation(spawnPoint.forward));
            newEnemy.SetWalkTarget(player.transform);
            lastSpawn = Time.time;
        }
    }
}
