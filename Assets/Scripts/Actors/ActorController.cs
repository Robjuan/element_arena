using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorController : MonoBehaviour
{
    [Header("Actor Components")]
    public Animator anim;
    public Health health ;
    public Damageable damage;
    public ManaUser mana;
    public StateController aiSateController;

    public abstract bool IsAlive { get; set; }
    
    
    public abstract void Die();
    public abstract void ReceiveDamage();
    

    void Awake()
    {
        /*
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        damage = GetComponent<Damageable>();
        mana = GetComponent<ManaUser>();
        aiSateController = GetComponent<StateController>();
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 

}
