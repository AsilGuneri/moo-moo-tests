using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
using Pathfinding;
using System.Threading;
using System;
using System.Threading.Tasks;

[RequireComponent(typeof(TargetController))]
[RequireComponent(typeof(PlayerDataHolder))]

public abstract class ABasicAttackController : NetworkBehaviour
{
    [SerializeField] protected NavMeshAgent agent;//
    [SerializeField] protected bool checkAuthority = true;
    [SerializeField] private float pathUpdateInterval = 1f; // Interval to update the path (in seconds)

    private Vector3 lastTargetPosition;
    private CancellationTokenSource cancellationTokenSource;

    private RichAI richAI;
    private bool isAttacking;

    protected TargetController tc;
    protected UnitMovementController umc;
    protected AnimationControllerBase ac;
    protected HeroBaseStatsData baseStats;

    //Additional Fields : Use these to increase attack speed etc. in game with temp. buff or cases like this. Note: Permanent upgrades needs to be saveable.
    protected float additionalAttackSpeed;
    protected float additionalRange;
    protected int additionalDamage;
    protected int additionalHp;


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
        richAI = GetComponent<RichAI>();
        umc = GetComponent<UnitMovementController>();
        ac = GetComponent<AnimationControllerBase>();

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
        if (!IsAttacking && isOutOfRange)
        {
            if(!isChasing)
            {
                StartChasing();
            }
        }
        if (counter >= (1 / baseStats.AttackSpeed) && !IsAttacking && !isOutOfRange)
        {
            StartAttacking();
        }
    }
    protected virtual async void StartChasing()
    {
        if (!isChasing)
        {
            isChasing = true;
            cancellationTokenSource = new CancellationTokenSource();
            await ChaseRoutine(cancellationTokenSource.Token);
        }
    }

    protected virtual void StopChasing()
    {
        if (isChasing)
        {
            isChasing = false;
            cancellationTokenSource.Cancel();
        }
    }

    private async Task ChaseRoutine(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (tc.Target != null)
            {
                Vector3 currentTargetPosition = tc.Target.transform.position;
                float distanceToTarget = Vector2.Distance(Extensions.To2D(currentTargetPosition), Extensions.To2D(transform.position));

                if (distanceToTarget > baseStats.Range)
                {
                    umc.ClientMove(currentTargetPosition, true, baseStats.Range);
                    lastTargetPosition = currentTargetPosition;
                }
            }
            else
            {
                StopChasing();
            }
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(pathUpdateInterval), cancellationToken);
            }
            catch (TaskCanceledException)
            {
                return;
            }
        }
    }

    protected virtual void ResetStoppingDistance()
    {
        //if (agent.stoppingDistance != 0) agent.stoppingDistance = 0;
    }
    protected abstract void StopAttacking();
    protected abstract void StartAttacking();


}
