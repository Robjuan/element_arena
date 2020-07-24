using UnityEngine.SceneManagement;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onActorDeath += HandleActorDeath;
    }

    void HandleActorDeath(GameObject deadActor)
    {
        if (deadActor.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        Debug.Log(deadActor + " has died");
    }

}
