using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(StatController))]
public abstract class UnitController : NetworkBehaviour
{
    
    //others
    public float AttackSpeed { get => attackSpeed; }
    public StatController StatController { get => statController; }
    public List<SkillController> Skills { get => skills; }
    public Vector3 HitPoint { get => transform.position + hitPointOffset; }
    public Transform ProjectileSpawnPoint { get => projectileSpawnPoint; }
    public Health Health { get => health; }
    public BasicAttackController AttackController { get => attackController; }
    public AnimationController AnimationController { get => animationController; }
    public NetworkAnimator NetworkAnimator { get => networkAnimator; }
    public Movement Movement { get => movement; }

    //temp variables
    public TargetController TargetController { get => targetController; }

    public UnitType unitType;
    public List<UnitType> enemyList;

    [SerializeField] protected Vector3 hitPointOffset;
    [SerializeField] protected Transform projectileSpawnPoint;
    [SerializeField] protected List<SkillController> skills = new List<SkillController>();



    protected AnimationController animationController;
    protected NetworkAnimator networkAnimator;
    protected TargetController targetController;
    protected BasicAttackController attackController;
    protected Movement movement;
    protected Health health;
    protected StatController statController;
    protected float attackSpeed;

    protected virtual void Awake()
    {
        CacheReferences();
        attackSpeed = statController.BaseStats.AttackSpeed;
    }
    public void ChangeAttackSpeed(float atkSpeed)
    {
        attackSpeed = atkSpeed;
    }

    protected bool IsEnemy(RaycastHit hitInfo)
    {
        if (hitInfo.transform.TryGetComponent(out UnitController unit))
        {
            return enemyList.Contains(unit.unitType);
        }
        return false;
    }
    public bool IsEnemyTo(UnitType unit)
    {
        return enemyList.Contains(unit);
    }
    private void CacheReferences()
    {
        attackController = GetComponent<BasicAttackController>();
        targetController = GetComponent<TargetController>();
        movement = GetComponent<Movement>();
        animationController = GetComponent<AnimationController>();
        networkAnimator = GetComponent<NetworkAnimator>();
        health = GetComponent<Health>();
        statController = GetComponent<StatController>();
    }
    protected void SubscribeEvents()
    {
        attackController.OnStartAttack += (() => { animationController.SetAttackStatus(true); animationController.TriggerAttack(); });
        attackController.OnEndAttack += (() => { animationController.SetAttackStatus(false); });
        attackController.OnAttackCancelled += (() =>
        {
            animationController.SetAttackStatus(false);
            animationController.SetAttackCancelled();
        });
        movement.OnMoveStart += (() => { animationController.SetMoveStatus(true); });
        movement.OnMoveStop += (() => { animationController.SetMoveStatus(false); });
    }

    [ClientRpc]
    public virtual void RpcOnRegister() { }

    [Server]
    public virtual void OnDeath(Transform killer) 
    {
        UnitManager.Instance.RemoveUnit(this);
    }

    public void StartUnit()
    {
        UnitManager.Instance.RegisterUnit(this);
        statController.InitializeStats();
        AnimationController.SetAttackSpeed(statController.BaseStats.AttackSpeed);

    }

}
