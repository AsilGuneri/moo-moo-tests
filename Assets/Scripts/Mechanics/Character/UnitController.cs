using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UnitController : NetworkBehaviour
{
    //others
    public List<Skill> Skills { get => skills; }
    public Transform ProjectileSpawnPoint { get => projectileSpawnPoint; }
    public Health Health { get => health; }
    public BasicAttackController AttackController { get => attackController; }
    public AnimationController AnimationController { get => animationController; }
    public NetworkAnimator NetworkAnimator { get => networkAnimator; }
    public Movement Movement { get => movement; }

    //temp variables
    public TargetController TargetController { get => targetController; }
    public float attackRange;
    public float attackSpeed;
    public UnitType unitType;
    public List<UnitType> enemyList;

    [SerializeField] protected Transform projectileSpawnPoint;
    [SerializeField] protected List<Skill> skills = new List<Skill>();



    protected AnimationController animationController;
    protected NetworkAnimator networkAnimator;
    protected TargetController targetController;
    protected BasicAttackController attackController;
    protected Movement movement;
    protected Health health;

    [Range(0, 1f)] public float animAttackPoint;


    protected virtual void Awake()
    {
        CacheReferences();
    }
    protected virtual void Start()
    {
        animationController.SetAttackSpeed(attackSpeed);
    }
    protected bool IsEnemy(RaycastHit hitInfo)
    {
        if (hitInfo.transform.TryGetComponent(out UnitController unit))
        {
            return enemyList.Contains(unit.unitType);
        }
        return false;
    }
    private void CacheReferences()
    {
        attackController = GetComponent<BasicAttackController>();
        targetController = GetComponent<TargetController>();
        movement = GetComponent<Movement>();
        animationController = GetComponent<AnimationController>();
        networkAnimator = GetComponent<NetworkAnimator>();
        health = GetComponent<Health>();
    }
    protected void SubscribeAnimEvents()
    {
        attackController.OnStartAttack += (() => { animationController.SetAttackStatus(true); });
        attackController.OnEndAttack += (() => { animationController.SetAttackStatus(false); });
        attackController.OnAttackCancelled += (() =>
        {
            animationController.SetAttackStatus(false);
            animationController.SetAttackCancelled();
        });
        movement.OnMoveStart += (() => { animationController.SetMoveStatus(true); });
        movement.OnMoveStop += (() => { animationController.SetMoveStatus(false); });
    }
}
