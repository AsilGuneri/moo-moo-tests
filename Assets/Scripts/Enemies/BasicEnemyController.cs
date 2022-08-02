using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyController : MonoBehaviour
{
    [SerializeField] private float range;


    protected NavMeshAgent _agent;
    protected Health _hc;
    protected GameObject _target;
    protected EnemyAnimationController _eac;

    private TargetController _tc;
    private bool isChasing;
    private bool isInAttackRange;

    private void Awake()
    {
        _tc = GetComponent<TargetController>();
        _agent = GetComponent<NavMeshAgent>();
        _eac = GetComponent<EnemyAnimationController>();
    }
    private void FixedUpdate()
    {
        PickTarget();
    }
    protected void PickTarget()
    {
        if (!_tc.Target)
        {
            _tc.Target = UnitManager.Instance.GetClosestUnit(transform.position);
        }

        else if (Vector2.Distance(Extensions.Vector3ToVector2(_tc.Target.transform.position), Extensions.Vector3ToVector2(transform.position)) < range)
            Attack();
        else Chase();
    }
    public void Activate()
    {
        _agent.enabled = true;
    }
    protected void Chase()
    {
        if (!_tc.Target || isInAttackRange) return;
        _eac.OnRun();
        Move(_tc.Target.transform.position);
        transform.LookAt(new Vector3(_tc.Target.transform.position.x, transform.position.y, _tc.Target.transform.position.z));
        if (!isChasing) isChasing = true;
    }
    protected void Attack()
    {
        transform.LookAt(new Vector3(_tc.Target.transform.position.x, transform.position.y, _tc.Target.transform.position.z));
        StopMove();
        _eac.OnAttack();
    }
    private void StopMove()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
    }
    private void Move(Vector3 destination)
    {
        _agent.isStopped = false;
        _agent.SetDestination(destination);

    }
}
