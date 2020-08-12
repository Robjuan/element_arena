using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
	private bool aiActive;

	public State currentState;
	public EnemyStats enemyStats;
	public Transform eyes;


	[HideInInspector] public NavMeshAgent navMeshAgent;
	//[HideInInspector] public Complete.TankShooting tankShooting;
	//	[HideInInspector] 
	public List<Transform> wayPointList;
	[HideInInspector] public int nextWayPoint;

	[HideInInspector] public Transform chaseTarget;

	void Awake()
	{
		//tankShooting = GetComponent<Complete.TankShooting>();
		navMeshAgent = GetComponent<NavMeshAgent>();
	}

	public void SetupAI(bool aiActivationFromTankManager) //, List<Transform> wayPointsFromTankManager)
	{
		//wayPointList = wayPointsFromTankManager;
		aiActive = aiActivationFromTankManager;
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
	
	void OnDrawGizmos()
    {
		if(currentState != null && eyes != null)
        {
			Gizmos.color = currentState.sceneGizmoColor;
			Gizmos.DrawWireSphere(eyes.position, enemyStats.lookSphereCastRadius);
        }
    }
}