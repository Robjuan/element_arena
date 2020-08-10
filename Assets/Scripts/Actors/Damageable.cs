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

    private EnemyController parent;

    void Awake()
    {
        // find the health component either at the same level, or higher in the hierarchy
        health = GetComponent<Health>();
        if (!health)
        {
            health = GetComponentInParent<Health>();
        }

        // currently this is working on NPCs but not on spiders?
        parent = GetComponent<EnemyController>();
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
            Debug.Log(parent);
            // even parent is null on spider??
            if (parent.anim != null)
            {
                parent.anim.SetTrigger("ReceiveDamage");
            }
        }
    }
}
