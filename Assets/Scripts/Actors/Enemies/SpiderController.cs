using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : EnemyController
{
    // set by statecontroller, reset 
    private Damageable attackTarget;

    // this will be called by AttackAction & the StateController
    public override void TryAttack(Collider other)
    {
        if (lastAttackTime + attackDelay < Time.time)
        {
            Debug.Log("trying attack: "+anim);
            anim.SetBool("Run", false);
            anim.SetTrigger("Attack");
            // inflict damage should happen at the end of the animation
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable)
            {
                attackTarget = damageable;
            }

            lastAttackTime = Time.time;
        }
        
    }

    // called by the animator on attack animation ends
    public void AttackEnd()
    {
        // need to check if target is still in range
        // raycast directly at target, if target is first hit and = player, then deal damage

        Debug.Log("attack ending");
        if (attackTarget != null) 
        {
            attackTarget.InflictDamage(attackDamage, this.gameObject);
            attackTarget = null;
        }
        
    }

    // called by the animator on die animation end
    public void DieParticle()
    {
        if (onDeathParticleEffect != null)
        {
            Instantiate(onDeathParticleEffect, transform.position, transform.rotation);
        }
    }

    public void DieEnd()
    {
        // this might break boss stuff?
        Destroy(this.gameObject);
    }

}
