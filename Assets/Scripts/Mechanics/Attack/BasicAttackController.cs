using Mirror;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public abstract class BasicAttackController : NetworkBehaviour
{
    public Action OnEachAttackStart;
    public Action OnActualAttackMoment;
    public Action OnStartAttack;
    public Action OnEndAttack;
    public Action OnAttackCancelled;

    public float AnimAttackPoint { get => animAttackPoint; }
    public bool IsAttacking => isAttacking;
   

    [Range(0f, 1f)]
    [SerializeField] protected float animAttackPoint;


    protected UnitController controller;
    bool isAttacking = false;
    bool isAttackStopped = false;
    int attackBlockCount = 0;
    HeroBaseStatsData baseStats;
    Coroutine autoAttackCoroutine; 


    // Public properties

    public int GetActualDamage()
    {
        int baseDmg = baseStats.Damage;
        int maxDmg = Mathf.CeilToInt(baseDmg * (1 + baseStats.AdditionalDamageRatio));
        int dmg = UnityEngine.Random.Range(baseDmg, maxDmg);
        if(controller.unitType == UnitType.Player) Debug.Log($"{name}'s base dmg : {baseDmg} , actual dmg : {dmg}");
        return dmg;
    }
    protected virtual void Awake()
    {
        controller = GetComponent<UnitController>();
    }
    private void Start()
    {
        baseStats = controller.StatController.BaseStats;
    }

    public void StartAutoAttack()
    {
        if (isAttacking) return;
        autoAttackCoroutine = StartCoroutine(AutoAttackRoutine());
    }
    public void StopAutoAttack()
    {
        if (!isAttacking) return;
        if (autoAttackCoroutine != null)
        {
            StopCoroutine(autoAttackCoroutine);
            autoAttackCoroutine = null;
            OnAttackEnd();
        }
    }

    private IEnumerator AutoAttackRoutine()
    {
        isAttacking = true;
        OnStartAttack?.Invoke();
        while (IsAutoAttackingAvailable())
        {
            Extensions.GetAttackTimes(controller.AttackSpeed, animAttackPoint, out float secondsBeforeAttack, out float secondsAfterAttack);

            RotateToTarget(controller.TargetController.Target.gameObject);

            OnEachAttackStart?.Invoke();
            OnAttackStart();
            yield return Extensions.GetWait(secondsBeforeAttack);

            RotateToTarget(controller.TargetController.Target.gameObject);
            OnAttackImpact();

            yield return Extensions.GetWait(secondsAfterAttack);
            OnEachAttackEnd();
        }
        OnAttackEnd();
    }
    void OnAttackEnd()
    {
        isAttacking = false;
        isAttackStopped = false;
        OnEndAttack?.Invoke();
    }

    private void RotateToTarget(GameObject target)
    {
        Transform targetTransform = target.transform;
        Vector3 lookPos = new Vector3(targetTransform.position.x, transform.position.y, targetTransform.position.z);
        if (controller.unitType != UnitType.Base && controller.unitType != UnitType.Tower)
        {
            transform.LookAt(lookPos);
        }
    }

    protected virtual bool IsAutoAttackingAvailable()
    {
        if (attackBlockCount > 0) return false;
        if (!controller.TargetController.HasTarget()) return false;
        if (controller.Health.IsDead) return false;
        if (GameFlowManager.Instance.CurrentState is GameState.GameEnd) return false;
        //if (Extensions.CheckRangeBetweenUnits(transform, controller.TargetController.Target.transform, controller.attackRange))
        //{
        //    return false;
        //}
        return true;
    }

    protected abstract void OnAttackStart();
    protected virtual void OnAttackImpact()
    {
        if (!IsAutoAttackingAvailable()) return;
        OnActualAttackMoment?.Invoke();

    }
    protected abstract void OnEachAttackEnd();


    public void ResetAttackController()
    {
        isAttacking = false;
    }
    public void BlockAttacking()
    {
        attackBlockCount++;
    }

    public void RemoveAttackingBlock()
    {
        attackBlockCount--;
    }
}
