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
        // could check if the target is still within range here?
        Debug.Log("attack ending");
        attackTarget.InflictDamage(attackDamage, this.gameObject);
        attackTarget = null;
    }

    protected void OnCollisionExit(Collision coll)
    {
        if (anim)
        {
            anim.SetBool("Attack", false);
        }
    }

}
