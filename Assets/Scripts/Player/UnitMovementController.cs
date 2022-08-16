using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MyBox;

public class UnitMovementController : MonoBehaviour
{
    [Separator("Script References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PlayerAnimationController pac;
    [SerializeField] private TargetController tc;
    [SerializeField] private BasicAttackController bac;

    private bool _isMoving;

    public void ClientMove(Vector3 pos, bool movingToTarget = false, float stoppingDistance = 0)
    {
        _isMoving = true;
        agent.isStopped = false;
        if (stoppingDistance != 0) agent.stoppingDistance = stoppingDistance;
        agent.SetDestination(pos);
        pac.OnMove();
        if(!movingToTarget) tc.SyncTarget(null);
    }
    public void ClientStop()
    {
        _isMoving = false;
        pac.OnStop();
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

    }
    public bool IsAgentMoving()
    {
        return (agent.hasPath && _isMoving);
    }
}
