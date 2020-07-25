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
        IsAlive = true;
    }

    public void SetWalkTarget(Transform wt)
    {
        this.walkTarget = wt;
    }

    public void Die()
    {
        if (IsAlive && this.gameObject)
        {
            IsAlive = false;
            Destroy(this.gameObject, 2f);
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
