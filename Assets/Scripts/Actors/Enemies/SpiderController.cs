using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : EnemyController
{
    public override void TryAttack(Collider other)
    {
        Debug.Log("attacking");
        anim.SetBool("Attack", true);
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable)
        {
            damageable.InflictDamage(attackDamage, this.gameObject);
        }
    }


    protected void OnCollisionExit(Collision coll)
    {
        if (anim)
        {
            anim.SetBool("Attack", false);
        }
    }

}
