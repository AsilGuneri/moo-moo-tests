using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UnitController : NetworkBehaviour
{
    //others
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

    protected AnimationController animationController;
    protected NetworkAnimator networkAnimator;
    protected TargetController targetController;
    protected BasicAttackController attackController;
    protected Movement movement;
    [Range(0, 1f)] public float animAttackPoint;


    protected virtual void Awake()
    {
        CacheReferences();
    }
    private void Start()
    {
       // animationController.SetAttackSpeed(attackSpeed);
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
    }
}
