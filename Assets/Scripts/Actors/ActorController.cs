using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Damageable))]
public abstract class ActorController : MonoBehaviour
{
    [Header("Actor Components")]
    [HideInInspector] public Animator anim;
    [HideInInspector] public Health health; // damage has RequireComponent health
    [HideInInspector] public Damageable damage;
    [HideInInspector] public ManaUser mana;

    public GameObject onDeathParticleEffect;

    public abstract bool IsAlive { get; set; }
    
    public abstract void Die();
    public abstract void ReceiveDamage(GameObject damageSource);


    public void Awake()
    {
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        damage = GetComponent<Damageable>();
        mana = GetComponent<ManaUser>();        
    }

}
