using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Mirror;
using UnityEngine.UIElements;

public class BasicAttackController : NetworkBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private float fireRate;
    [SerializeField] private PlayerAnimationController pac;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float range;

    [SerializeField] private int damage;

    private Health _target;
    private float counter;
    private GameObject _lastProjectile;

    public Health Target
    {
        get => _target;
        set => _target = value;
    }
    public bool IsAttacking
    {
        get;set;
    }

    #region Server
    [Server]
    private bool CanFireAtTarget()
    {
        return (_target.transform.position - transform.position).sqrMagnitude > attackRange * attackRange;
    }
    [Command]
    private void CmdSpawnProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetupProjectile(_target.gameObject, damage);

        NetworkServer.Spawn(projectile, connectionToClient);
    }
    #endregion
    #region Client
    [ClientCallback]
    private void Update()
    {
        if (counter <= fireRate) counter += Time.deltaTime;
        if (!_target) { agent.stoppingDistance = 0; return; }
        if (Vector2.Distance(Extensions.Vector3ToVector2(_target.transform.position), Extensions.Vector3ToVector2(transform.position)) > range)
        {
            agent.stoppingDistance = range;
            agent.SetDestination(_target.transform.position);
        }
        else if (counter >= fireRate)
        {
            transform.LookAt(new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z));
            pac.Animate("Shoot", true);
          //  CmdSpawnProjectile(_target.gameObject);
            counter = 0;
        }
       
    }
    #endregion

    /*[SerializeField] private NavMeshAgent navMeshAgent;
    
    public UnityEvent OnAttackStart;
    public UnityEvent OnAttackEnd;

    private int _basicDamage = 1;
    private float _basicCoolDown = 1f;
    private bool _basicAttackReady = true;
    private bool _isCounting = false;
    private bool _isAttacking = false;
    private bool _isAutoAttacking = false;
    private float _timer = 0;
    private Health _hc;
    private PlayerMertController _pc;
    
    public bool IsAttacking { get => _isAttacking; }
    public bool IsAutoAttacking 
    {
        get => _isAutoAttacking;
        set => _isAutoAttacking = value;
    }

    private void Start()
    {
       // if (!hasAuthority) return;

        UpdateCooldown();
        OnAttackEnd.AddListener(EndBasicAttack);
        _pc = GetComponent<PlayerMertController>();
    }

    private void FixedUpdate()
    {
        //if (!hasAuthority) return;

        if (_isCounting)
        {
            _timer+= Time.deltaTime;
            if (_timer >= _basicCoolDown) EndBasicCoolDown();
        }
    }
    public virtual void BasicAttack(Health hc, string lastState)
    {

        if (!_basicAttackReady) return;
        _isAutoAttacking = true;
        _isAttacking = true;
        if (navMeshAgent.hasPath)
        {
            navMeshAgent.SetDestination(transform.position);
            //AnimationManager.Instance.ChangeAnimationState("BasicAttack", animator, lastState);
            _pc.Animate("Shoot", true);

        }
        _hc = hc;

        //AnimationManager.Instance.ChangeAnimationState("BasicAttack", animator, lastState);
        _pc.Animate("Shoot", true);
        transform.LookAt(hc.transform);
        hc.TakeDamage(_basicDamage);
        DealDamage();
        StartBasicCooldown();
    }
    public void DealDamage()
    {
        var arrow = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
        arrow.Target = _hc;
    }
    public void UpdateCooldown() // CALL THIS METHOD WHENEVER YOU CHANGE ATTACKSPEED
    {
        _basicCoolDown =  1 / GetComponent<PlayerMertController>().AttackSpeed;
    }

    public void InvokeAttackEnded()
    {
        OnAttackEnd?.Invoke();
    }
    public void InvokeAttackStart()
    {
        OnAttackStart?.Invoke();
    }

    private void StartBasicCooldown()
    {
        _isCounting = true;
        _basicAttackReady = false;
        _timer = 0;
    }
    private void EndBasicCoolDown()
    {
        _basicAttackReady = true;
        if (_isAutoAttacking) BasicAttack(_hc, "Shoot");
    }
    private void EndBasicAttack()
    {
        _isAttacking = false;
     //   StartCoroutine(nameof(RepeatBasicAttack));
    }
    private IEnumerator RepeatBasicAttack()
    {
        yield return new WaitUntil(() => _basicAttackReady);
        BasicAttack(_hc, "Shoot");
    }
    */
}
