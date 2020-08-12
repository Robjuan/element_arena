using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

[RequireComponent(typeof(StateController))]
public class EnemyController : ActorController
{
    [Header("Enemy Components")]
    [HideInInspector] public StateController aiSateController;
    [HideInInspector] public override bool IsAlive { get; set; }


    [Header("Stats")]
    public float touchDamage;

    private void Awake() 
    {
        aiSateController = GetComponent<StateController>();

        IsAlive = true;
        aiSateController.SetupAI(true);
    }

    public override void Die()
    {
        if (IsAlive)
        {
            IsAlive = false;
            aiSateController.Deactivate();

            anim.SetTrigger("Die");
            this.GetComponent<BoxCollider>().enabled = false;
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
