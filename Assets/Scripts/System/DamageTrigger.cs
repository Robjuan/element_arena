using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageTrigger : MonoBehaviour
{

    [Tooltip("Damage per tick")]
    public float damagePerTick;

    [Tooltip("Delay between damage ticks")]
    public float tickDelay;

    private List<Damageable> currentContainedDamageables = new List<Damageable>();
    private float m_LastTickTime = Mathf.NegativeInfinity;

    void Update()
    {
        // everything will tick the same presently, not tracking their own timings based on enter
        // consider changing to per-second , or storing damageables as a dict (so each can have it's own time start)
        if ((m_LastTickTime + tickDelay) < Time.time)
        {
            foreach (var dmg in currentContainedDamageables)
            {
                if(dmg != null)
                {
                    dmg.InflictDamage(damagePerTick, this.gameObject);
                }
            }
            m_LastTickTime = Time.time;
        }
    }

    private Damageable getDamageable(Collider col)
    {
        var targetcol = col.gameObject;

        // check the obj, then the children
        var dmg = targetcol.GetComponent<Damageable>();
        if (!dmg)
        {
            dmg = targetcol.GetComponentInChildren<Damageable>();
        }
        if (!dmg)
        {
            return null;
        }
        return dmg;
    }


    private void OnTriggerEnter(Collider other)
    {
        var dmg = getDamageable(other);
        if (dmg != null){
            currentContainedDamageables.Add(dmg);
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        var dmg = getDamageable(other);
        if (dmg != null){
            currentContainedDamageables.Remove(dmg);
        }
    }
}
