using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
	private bool aiActive;

	public State currentState;
	public State remainState;

	public Transform eyes;
	public List<Transform> wayPointList;

	// information from parent EnemyController
	[HideInInspector] public NavMeshAgent navMeshAgent;
	[HideInInspector] public EnemyController parent;

	// cross-state info
	[HideInInspector] public int nextWayPoint;
	[HideInInspector] public Transform chaseTarget; // set in LookDecision
	[HideInInspector] public float stateTimeElapsed;

	public void SetupAI(bool aiActiveParam, NavMeshAgent EnemyControllerNavMeshAgent, EnemyController parentController)
	{
		parent = parentController;
		navMeshAgent = EnemyControllerNavMeshAgent;

		aiActive = aiActiveParam;
		if (aiActive)
		{
			navMeshAgent.enabled = true;
		}
		else
		{
			navMeshAgent.enabled = false;
		}
	}

	public void Deactivate()
    {
		// this will cause it to finish teh current action and then stop
		// need to actually move it to some kind of "die" action 
		aiActive = false;
    }

	void Update()
    {
		if(!aiActive)
        {
			return;
        }

		currentState.UpdateState(this);
    }
	
	public void TransitionToState(State nextState)
    {
		if (nextState != remainState)
        {
			currentState = nextState;
        }
    }

	void OnDrawGizmos()
    {
		if(currentState != null && eyes != null && parent != null)
        {
			Gizmos.color = currentState.sceneGizmoColor;
			Gizmos.DrawWireSphere(eyes.position, parent.lookSphereCastRadius);
        }
    }

	public bool CheckCountDownElapsed(float duration)
    {
		stateTimeElapsed += Time.deltaTime;
		return (stateTimeElapsed >= duration);
    }

	private void OnExitState()
    {
		stateTimeElapsed = 0;
    }
	
}