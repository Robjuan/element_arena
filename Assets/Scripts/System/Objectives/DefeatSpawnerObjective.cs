using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatSpawnerObjective : Objective
{
    public int spawnersToDefeat;
    public EnemySpawner[] allSpawners;

    private int currentAliveSpawners;

    void Start()
    {
        GameEvents.current.onSpawnerEmpty += CheckSpawnerCount;
        currentAliveSpawners = allSpawners.Length;
    }

    public override bool IsCompleted()
    {
        return (currentAliveSpawners < spawnersToDefeat);
    }

    public override void Complete()
    {
        Debug.Log("you have completed objective :" + this);
    }

    void CheckSpawnerCount(GameObject deadSpawner)
    {
        currentAliveSpawners -= 1;
        
    }
}
