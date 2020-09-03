using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowManager : MonoBehaviour
{
    public Image winMessage;
    public Image loseMessage;

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
            winMessage.gameObject.SetActive(true);
            Invoke("LoadScene", 4f);
        }
    }

    void LoadScene()
    {
        // 0 is the first scene in our build index, which is our menu scene
        winMessage.gameObject.SetActive(false);
        loseMessage.gameObject.SetActive(false);
        SceneManager.LoadScene(0);        
    }

    void HandleActorDeath(GameObject deadActor)
    {
        if (deadActor.CompareTag("Player"))
        {
            loseMessage.gameObject.SetActive(true);
            Invoke("LoadScene", 4f);
        }
        //Debug.Log(deadActor + " has died");
    }



}
