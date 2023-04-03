using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MyBox;
using Mirror;

public class BasicEnemyController : NetworkBehaviour
{
    protected NavMeshAgent _agent;
    protected Health _hc;
    protected GameObject _target;
    protected AnimationControllerBase _ac;
    private TargetController _tc;


    private void Awake()
    {
        _tc = GetComponent<TargetController>();
        _agent = GetComponent<NavMeshAgent>();
        _ac = GetComponent<AnimationControllerBase>();
    }
    private void FixedUpdate()
    {
        if (_tc.Target == null) PickTarget();
    }
    protected void PickTarget()
    {
        if (!_tc.Target)
        {
            _tc.Target = UnitManager.Instance.GetClosestUnit(transform.position);
        }
    }
    public void Activate()
    {
        _agent.enabled = true;
    }
}
