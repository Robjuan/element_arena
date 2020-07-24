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
        Debug.Log("da id :" + deadActor.GetInstanceID() + " local : " + GetInstanceID());

        // todo: check null here, also why are nulls getting sent
        if (deadActor == this.gameObject)
        {
            Debug.Log("Destroying " + deadActor);
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
