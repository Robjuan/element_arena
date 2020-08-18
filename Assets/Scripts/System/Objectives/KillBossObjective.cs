using UnityEngine;

public class KillBossObjective : Objective
{
    public ActorController boss;

    public override bool IsCompleted()
    {
        return !boss.IsAlive;
    }

    public override void Complete()
    {
        Debug.Log("you have completed objective: " + this);
    }
}
