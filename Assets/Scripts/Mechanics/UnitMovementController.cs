using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MyBox;

public class UnitMovementController : MonoBehaviour
{
    [Separator("Script References")]
    [SerializeField] private AnimationController bac;
    private TargetController tc;
    private CustomAgentController agentController;
    private void Awake()
    {
        tc = GetComponent<TargetController>();
        agentController = GetComponent<CustomAgentController>();
    }

    public void ClientMove(Vector3 pos, bool movingToTarget = false, float stoppingDistance = 0)
    {
        //agent.isStopped = false;
        //if (stoppingDistance != 0) agent.stoppingDistance = stoppingDistance;
        //agent.SetDestination(pos);
        if (bac != null) 
        {
            bac.OnAttackEnd();
            bac.OnMove();
        }
        if(!movingToTarget) tc.SyncTarget(null);
        agentController.SetTarget(pos);
    }
    public void ClientStop()
    {
        if(bac != null) bac.OnStop();
        //agent.isStopped = true;
        //agent.velocity = Vector3.zero;
        agentController.SetTarget(Vector3.zero);

    }
}
