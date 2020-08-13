using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public int spawnersToDefeat;
    public EnemySpawner[] allSpawners;

    private int currentAliveSpawners;

    void Start()
    {
        GameEvents.current.onActorDeath += HandleActorDeath;
        GameEvents.current.onSpawnerEmpty += CheckSpawnerCount;

        currentAliveSpawners = allSpawners.Length;
    }

    void LoadScene()
    {
        // 0 is the first scene in our build index, which is our menu scene
        SceneManager.LoadScene(0);        
    }

    void HandleActorDeath(GameObject deadActor)
    {
        if (deadActor.CompareTag("Player"))
        {
            Debug.Log("You have died.");
            Invoke("LoadScene", 0.5f);
        }
        //Debug.Log(deadActor + " has died");
    }

    void CheckSpawnerCount(GameObject deadSpawner)
    {
        currentAliveSpawners -= 1;
        if (currentAliveSpawners < spawnersToDefeat)
        {
            Debug.Log("You have won!!");
            Invoke("LoadScene", 1f);
        }
    }

}
