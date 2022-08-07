using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MyBox;
using Mirror;

public class BasicEnemyController : NetworkBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float attackSpeed;

    [Separator("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpawnDelay;
    [SerializeField] private int damage;
    [SerializeField] private Transform projectileSpawnPoint;


    protected NavMeshAgent _agent;
    protected Health _hc;
    protected GameObject _target;
    protected EnemyAnimationController _eac;

    private TargetController _tc;
    private bool isChasing;
    private bool isInAttackRange;

    private float _counter = 0;
    private bool _canAttack = false;

    private void Awake()
    {
        _tc = GetComponent<TargetController>();
        _agent = GetComponent<NavMeshAgent>();
        _eac = GetComponent<EnemyAnimationController>();
    }

    private void FixedUpdate()
    {
        if (_counter < attackSpeed)
        {
            _counter += Time.fixedDeltaTime;
            _canAttack = false;
        }
        else
        {
            _canAttack = true;
            _counter = 0;
        }
        if (UnitManager.Instance.Players.Count > 0) PickTarget();
        else StopMove();
    }
    protected void PickTarget()
    {
        if (!_tc.Target)
        {
            _tc.Target = UnitManager.Instance.GetClosestUnit(transform.position);
        }
        else if (Vector2.Distance(Extensions.Vector3ToVector2(_tc.Target.transform.position), Extensions.Vector3ToVector2(transform.position)) < range)
        {
            StopMove();
            if (_canAttack) Attack();
        }

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
        StartCoroutine(nameof(SpawnProjectileRoutine));
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
    private IEnumerator SpawnProjectileRoutine()
    {
        yield return new WaitForSeconds(projectileSpawnDelay);
        SpawnProjectile();
    }
    [ServerCallback]
    private void SpawnProjectile()
    {
        var projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetupProjectile(_tc.Target, damage);
        NetworkServer.Spawn(projectile);
        //asdasd
    }    
}
