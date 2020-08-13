using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return Look(controller);
    }

    private bool Look(StateController controller)
    {
        // todo: replace with cone for vision?
        // if something comes in the cone then raycast it to check for sure?
        RaycastHit hit;

        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.parent.lookRange, Color.green);

        if(Physics.SphereCast(controller.eyes.position, controller.parent.lookSphereCastRadius, controller.eyes.forward, out hit, controller.parent.lookRange)
            && hit.collider.CompareTag("Player"))
        {
            controller.chaseTarget = hit.transform;
            return true;
        } else
        {
            return false;
        }
    }
}
