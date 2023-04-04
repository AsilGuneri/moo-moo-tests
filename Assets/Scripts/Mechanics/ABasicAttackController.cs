using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

[RequireComponent(typeof(TargetController))]
[RequireComponent(typeof(PlayerDataHolder))]

public abstract class ABasicAttackController : NetworkBehaviour
{
    [SerializeField] protected NavMeshAgent agent;//
    [SerializeField] protected bool checkAuthority = true;
    [SerializeField] protected bool isStable = false;


    protected TargetController tc;
    protected UnitMovementController umc;
    protected AnimationControllerBase ac;
    protected HeroBaseStatsData baseStats;

    //Additional Fields : Use these to increase attack speed etc. in game with temp. buff or cases like this. Note: Permanent upgrades needs to be saveable.
    protected float additionalAttackSpeed;
    protected float additionalRange;
    protected int additionalDamage;
    protected int additionalHp;


    private bool isAttacking;
    protected bool isChasing;

    protected float counter;

    public float AttackSpeed { get { return baseStats.AttackSpeed; } set { baseStats.AttackSpeed = value; } }
    public float Range { get { return baseStats.Range; } set { baseStats.Range = value; } }
    public bool IsAttacking 
    { 
        get 
        {
            return isAttacking; 
        } 
        protected set 
        { 
            isAttacking = value;
            if (value) ac.OnAttackStart(baseStats.AttackSpeed);
            else ac.OnAttackEnd();
        } 
    }

    protected virtual void Awake()
    {
        tc = GetComponent<TargetController>();
        baseStats = GetComponent<PlayerDataHolder>().HeroStatsData;
        if(!isStable)
        {
            umc = GetComponent<UnitMovementController>();
            ac = GetComponent<AnimationControllerBase>();
        }
    }
    [ClientCallback]
    protected virtual void Update()
    {
        if (checkAuthority && !hasAuthority) return;
        if (counter <= (1 / baseStats.AttackSpeed)) counter += Time.deltaTime;
        if (tc.Target == null)
        {
            if (IsAttacking)
            {
                StopAttacking();
            }
            if (!IsAttacking)
            {
                ResetStoppingDistance();
                StopChasing();
            }
            return;
        }
        bool isOutOfRange = Vector2.Distance(Extensions.To2D(tc.Target.transform.position),
            Extensions.To2D(transform.position)) > baseStats.Range;
        if (!isStable && !IsAttacking && isOutOfRange)
        {
            ChaseToAttack();
        }
        else if (counter >= (1 / baseStats.AttackSpeed) && !IsAttacking)
        {
            StartAttacking();
        }
    }
    protected virtual void ChaseToAttack()
    {
        umc.ClientMove(tc.Target.transform.position, true, baseStats.Range);
        isChasing = true;
    }
    protected virtual void StopChasing()
    {
        if (isChasing)
        {
            umc.ClientStop();
            isChasing = false;
        }
    }
    protected virtual void ResetStoppingDistance()
    {
        //if (agent.stoppingDistance != 0) agent.stoppingDistance = 0;
    }
    protected abstract void StopAttacking();
    protected abstract void StartAttacking();
    

}
