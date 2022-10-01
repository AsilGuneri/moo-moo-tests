using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

[RequireComponent(typeof(TargetController))]
[RequireComponent(typeof(UnitMovementController))]
[RequireComponent(typeof(PlayerDataHolder))]

public abstract class ABasicAttackController : NetworkBehaviour
{
    [SerializeField] protected NavMeshAgent agent;//
    [SerializeField] protected bool checkAuthority = true;


    protected TargetController tc;
    protected UnitMovementController umc;
    protected AnimationController ac;
    protected HeroBaseStatsData stats;

    //Additional Fields : Use these to increase attack speed etc. in game with temp. buff or cases like this. Note: Permanent upgrades needs to be saveable.
    protected float additionalAttackSpeed;
    protected float additionalRange;
    protected int additionalDamage;
    protected int additionalHp;


    protected bool isAttacking;
    protected bool isChasing;

    protected float counter;

    public float AdditionalAttackSpeed { get { return additionalAttackSpeed; } set { additionalAttackSpeed = value; } }
    public int AdditionalDamage { get { return additionalDamage; } set { additionalDamage = value; } }
    public float AttackSpeed { get { return stats.AttackSpeed; } set { stats.AttackSpeed = value; } }
    public float Range { get { return stats.Range; } set { stats.Range = value; } }

    protected virtual void Awake()
    {
        tc = GetComponent<TargetController>();
        umc = GetComponent<UnitMovementController>();
        ac = GetComponent<AnimationController>();
        stats = GetComponent<PlayerDataHolder>().HeroStatsData;
    }
    [ClientCallback]
    protected virtual void Update()
    {
        if (checkAuthority && !hasAuthority) return;
        if (counter <= (1 / stats.AttackSpeed)) counter += Time.deltaTime;
        if (tc.Target == null)
        {
            StopAttacking();
            ResetStoppingDistance();
            StopChasing();
            return;
        }
        if (Vector2.Distance(Extensions.Vector3ToVector2(tc.Target.transform.position), Extensions.Vector3ToVector2(transform.position)) > stats.Range && !isAttacking)
        {
            ChaseToAttack();
        }
        else if (counter >= (1 / stats.AttackSpeed) && !isAttacking)
        {
            StartAttacking();
        }
    }
    protected virtual void ChaseToAttack()
    {
        if (ac != null) ac.OnAttackEnd();
        umc.ClientMove(tc.Target.transform.position, true, stats.Range);
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
        if (agent.stoppingDistance != 0) agent.stoppingDistance = 0;
    }
    protected abstract void StopAttacking();
    protected abstract void StartAttacking();
    

}
