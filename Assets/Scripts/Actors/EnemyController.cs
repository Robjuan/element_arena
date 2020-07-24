using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Transform walkTarget;
    public float walkSpeed;
    public bool IsAlive { get; set; }

    private void Start() 
    {
        GameEvents.current.onActorDeath += Die;
        IsAlive = true;
    }

    public void SetWalkTarget(Transform wt)
    {
        this.walkTarget = wt;
    }

    void Die(GameObject deadActor)
    {
        if (IsAlive && deadActor == this.gameObject)
        {
            IsAlive = false;
            Destroy(deadActor, 2f);
        }

    }

    void Update()
    {
        if (IsAlive)
        {
            transform.LookAt(walkTarget.transform);
            transform.position = Vector3.MoveTowards(transform.position, walkTarget.position, walkSpeed * Time.deltaTime);
        }
    }

}
