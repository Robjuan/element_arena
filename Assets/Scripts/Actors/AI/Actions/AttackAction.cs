using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
public class AttackAction : Action
{
    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        RaycastHit hit;

        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.parent.attackRange, Color.red);

        if (Physics.SphereCast(controller.eyes.position, controller.parent.lookSphereCastRadius, controller.eyes.forward, out hit, controller.parent.attackRange)
            && hit.collider.CompareTag("Player"))
        {
            // doesn't seem to attack repeatedly if you're in place?
            // potentially replace with collider/trigger in front then raycast check
            controller.parent.TryAttack(hit.collider);
        }
        else // potentially wouldn't need this if patrolaction was last in list?
        {
            controller.parent.anim.SetBool("Run", true);
        }
    }
}
