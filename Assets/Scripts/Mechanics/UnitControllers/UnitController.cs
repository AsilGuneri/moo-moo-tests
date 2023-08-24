using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UnitController : NetworkBehaviour
{
    public Dictionary<string,SkillController> SkillControllerDictionary = new Dictionary<string, SkillController>();
    //others
    public Vector3 HitPoint { get => transform.position + hitPointOffset; }
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

    [SerializeField] protected Vector3 hitPointOffset;
    [SerializeField] protected Transform projectileSpawnPoint;
    [SerializeField] protected List<Skill> skills = new List<Skill>();



    protected AnimationController animationController;
    protected NetworkAnimator networkAnimator;
    protected TargetController targetController;
    protected BasicAttackController attackController;
    protected Movement movement;
    protected Health health;

    protected virtual void Awake()
    {
        CacheReferences();
        InitializeSkills();
    }
    
    protected virtual void Start()
    {
        health.OnDeath += () => { targetController.SetTarget(null); };
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
    public void UseSkill(Skill skill)
    {
        SkillControllerDictionary[skill.name].Use();
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
    private void InitializeSkills()
    {
        foreach (var skill in skills)
        {
            skill.Initialize(transform);
        }
    }
}