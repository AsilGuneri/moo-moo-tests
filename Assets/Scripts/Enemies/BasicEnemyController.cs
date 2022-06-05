using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyController : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;
    protected Health _hc;
    protected Health _target;

    private bool isChasing;
    private bool isInAttackRange;

    private void FixedUpdate()
    {
        PickTarget();
    }
    private void PickTarget()
    {
        if (!_target) _target = UnitManager.Instance.GetClosestUnit(transform.position);
        else Chase();
    }
    protected void ChooseTarget()
    {

    }
    protected void Chase()
    {
        if (!_target || isInAttackRange) return;
        agent.SetDestination(_target.transform.position);
        if (!isChasing) isChasing = true;
    }
    protected void Attack()
    {

    }
}
