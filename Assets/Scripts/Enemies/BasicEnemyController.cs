using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyController : MonoBehaviour
{
    protected NavMeshAgent _agent;
    protected Health _hc;
    protected GameObject _target;

    private TargetController _tc;
    private bool isChasing;
    private bool isInAttackRange;

    private void Awake()
    {
        _tc = GetComponent<TargetController>();
        _agent = GetComponent<NavMeshAgent>();
    }
    private void FixedUpdate()
    {
        PickTarget();
    }
    private void PickTarget()
    {
        if (!_tc.Target) _tc.Target= UnitManager.Instance.GetClosestUnit(transform.position);
        else Chase();
    }
    public void Activate()
    {
        _agent.enabled = true;
    }
    protected void ChooseTarget()
    {

    }
    protected void Chase()
    {
        if (!_tc.Target || isInAttackRange) return;
        _agent.SetDestination(_tc.Target.transform.position);
        if (!isChasing) isChasing = true;
    }
    protected void Attack()
    {

    }
}
