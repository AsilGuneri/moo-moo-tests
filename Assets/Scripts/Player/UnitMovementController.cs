using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MyBox;

public class UnitMovementController : MonoBehaviour
{
    [Separator("Script References")]
    private NavMeshAgent agent;
    [SerializeField] private BasicAnimationController bac;
    private TargetController tc;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        tc = GetComponent<TargetController>();
    }

    public void ClientMove(Vector3 pos, bool movingToTarget = false, float stoppingDistance = 0)
    {
        agent.isStopped = false;
        if (stoppingDistance != 0) agent.stoppingDistance = stoppingDistance;
        agent.SetDestination(pos);
        if(bac != null) bac.OnMove();
        if(!movingToTarget) tc.SyncTarget(null);
    }
    public void ClientStop()
    {
        if(bac != null) bac.OnStop();
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

    }
}
