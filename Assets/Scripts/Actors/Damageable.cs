﻿using UnityEngine;

[RequireComponent(typeof(Health))]
public class Damageable : MonoBehaviour
{
    [Tooltip("Multiplier to apply to the received damage")]
    public float damageMultiplier = 1f;
    [Range(0, 1)]
    [Tooltip("Multiplier to apply to self damage")]
    public float sensibilityToSelfdamage = 0.5f;

    public Health health { get; private set; }

    private ActorController parent;

    void Start()
    {
        health = GetComponent<Health>();
        parent = GetComponent<ActorController>();
    }

    public void InflictDamage(float damage, GameObject damageSource)
    {

        if(health)
        {
            var totalDamage = damage;

            // potentially reduce damages if inflicted by self
            if (health.gameObject == damageSource)
            {
                totalDamage *= sensibilityToSelfdamage;
            }

            // apply the damages
            health.TakeDamage(totalDamage, damageSource);
            if(parent)
            {
                parent.ReceiveDamage(damageSource);
            }
        }
    }
}
