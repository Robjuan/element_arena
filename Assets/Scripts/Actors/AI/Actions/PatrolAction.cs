using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]

public class PatrolAction : Action
{
    public override void Act(StateController controller)
    {
        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {
        controller.navMeshAgent.SetDestination(controller.wayPointList[controller.nextWayPoint].position);
        controller.navMeshAgent.isStopped = false;

        controller.parent.anim.SetBool("Run", true);

        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance 
            && !controller.navMeshAgent.pathPending)
        {
            // we have arrived, increase waypoint but don't go past the end, go back to the start
            controller.nextWayPoint = (controller.nextWayPoint + 1) % controller.wayPointList.Count;
        }

    }

}
