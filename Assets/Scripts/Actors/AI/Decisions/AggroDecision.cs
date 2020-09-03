using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Aggro")]
public class AggroDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return IsAggrod(controller);
    }

    private bool IsAggrod(StateController controller)
    {
        return controller.isAggrod;
    }

}
