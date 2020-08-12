using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : EnemyController
{
   
    // todo: have the statecontroller handle attacking
    protected void OnCollisionEnter(Collision coll)
    {
        var other = coll.gameObject;
        // todo: let them damage each other?
        // maybe with better nav and more varied attacks
        if (other.tag == "Player")
        {
            if (anim)
            {
                anim.SetBool("Attack", true);
            }

            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable)
            {
                damageable.InflictDamage(touchDamage, this.gameObject);
            }
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
