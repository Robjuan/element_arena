using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public List<Objective> objectives;
    private List<Objective> activeObjectives;

    private bool playerWin;
   
    void Start()
    {
        GameEvents.current.onActorDeath += HandleActorDeath;
        activeObjectives = new List<Objective>(objectives);
}

    void Update()
    {
        if (activeObjectives.Count > 0)
        {
            // iterate list in reverse to allow removing
            for (int i = activeObjectives.Count - 1; i >= 0; i--)
            {
                if(activeObjectives[i].IsCompleted())
                {
                    activeObjectives[i].Complete();
                    activeObjectives.RemoveAt(i);
                }
            }
        } else
        {
            Debug.Log("you completed all objectives and won!!");
            Invoke("LoadScene", 2f);
        }
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



}
