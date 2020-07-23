using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Transform walkTarget;
    
    public bool isAlive;
    public float walkSpeed;
   
    private void Start() 
    {
        GameEvents.current.onActorDeath += Die;
    }

    public void SetWalkTarget(Transform wt)
    {
        this.walkTarget = wt;
    }

    void Die(GameObject go)
    {
        // todo: 
        Debug.Log("Destroying " + go);
        isAlive = false;
        Destroy(go, 2f);
    }

    void Update()
    {
        if (isAlive)
        {
            transform.LookAt(walkTarget.transform);
            transform.position = Vector3.MoveTowards(transform.position, walkTarget.position, walkSpeed * Time.deltaTime);
        }
    }

}
