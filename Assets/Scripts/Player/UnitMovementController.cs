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

    public void ClientMove(Vector3 pos)
    {
        agent.isStopped = false;
        agent.SetDestination(pos);
        if (pac.CurrentAnimState != "Run") pac.Animate("Run", false);
        tc.SyncTarget(null);
        bac.IsAttacking = false;
    }
    public void ClientStop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        pac.Animate("Idle", false);
    }
}
