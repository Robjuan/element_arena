using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public int spawnersToDefeat;
    public EnemySpawner[] allSpawners;

    private int currentAliveSpawners;

    void Awake()
    {
        GameEvents.current.onActorDeath += HandleActorDeath;
        GameEvents.current.onSpawnerEmpty += CheckSpawnerCount;

        currentAliveSpawners = allSpawners.Length;
    }

    void LoadScene()
    {
        // todo: take in scene as param or do this better
        // this isn't firing weaponchange events and stuff
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);        
    }

    void HandleActorDeath(GameObject deadActor)
    {
        if (deadActor.tag == "Player")
        {
            Debug.Log("You have died.");
            Invoke("LoadScene", 5f);
        }
        //Debug.Log(deadActor + " has died");
    }

    void CheckSpawnerCount(GameObject deadSpawner)
    {
        currentAliveSpawners -= 1;
        if (currentAliveSpawners < spawnersToDefeat)
        {
            Debug.Log("You have won!!");
            Invoke("LoadScene", 5f);
        }
    }

}
