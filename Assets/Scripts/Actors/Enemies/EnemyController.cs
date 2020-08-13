using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.WSA.Input;

[RequireComponent(typeof(StateController), typeof(NavMeshAgent))]
public class EnemyController : ActorController
{
    [Header("Enemy Components")]
    [HideInInspector] public StateController aiSateController;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public override bool IsAlive { get; set; }


    // movement stats are set on the NavMeshAgent in the prefab

    [Header("Vision Stats")]
    public float lookRange;
    public float lookSphereCastRadius;

    [Header("Combat Stats")]
    public float attackDamage;
    public float attackSpeed;
    public float attackRange;

    public virtual void TryAttack(Collider other)
    {
        Debug.Log("Enemy Controller base class does not implement TryAttack");
    }

    public new void Awake() 
    {
        base.Awake();
        aiSateController = GetComponent<StateController>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        IsAlive = true;
        aiSateController.SetupAI(true, navMeshAgent, this);
    }

    public override void Die()
    {
        if (IsAlive)
        {
            IsAlive = false;
            //aiSateController.Deactivate();
            anim.SetTrigger("Die");
            //this.GetComponent<BoxCollider>().enabled = false;
            //Destroy(this.gameObject, 2f);
        }
    }

    public override void ReceiveDamage()
    {
        if(anim)
        {
            anim.SetTrigger("ReceiveDamage");
        }
    }
}
