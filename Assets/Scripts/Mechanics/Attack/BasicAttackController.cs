using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BasicAttackController : NetworkBehaviour
{
    [Range(0f,1f)]
    [SerializeField] protected float animAttackPoint;

    /// <summary>
    /// For range : projectile spawn moment, for melee : hit moment.
    /// </summary>
    public Action OnActualAttackMoment;
    public Action AfterLastAttack;
    public bool IsSetToStopAfterAttack { get => isSetToStopAfterAttack; }
    public bool IsAttacking { get => isAttacking; }

    protected UnitController controller;
    protected bool isAttacking;

    private bool isAttackStopped; //just don't set to false anywhere else, you can set to true if needed

    public Action OnStartAttack;
    public Action OnEndAttack;
    public Action OnAttackCancelled;

    private Task attackTask = null;
    private bool isSetToStopAfterAttack;
    private int attackBlockCount = 0;
    //temp
    public int Damage;
    private bool isCurrentlyAttacking = false;


    protected virtual void Awake()
    {
        controller = GetComponent<UnitController>();
    }

    public async void StartAutoAttack()
    {
        if (isAttacking) return;

        isAttacking = true;
        OnStartAttack?.Invoke();

        while (IsAutoAttackingAvailable()) await AttackOnce();

        isAttacking = false;
        isAttackStopped = false;
        OnEndAttack?.Invoke();
    }
    public void BlockAttacking()
    {
        attackBlockCount++;
    }
    public void RemoveAttackingBlock()
    {
        attackBlockCount--;
    }
    /// <summary>
    /// for enemies
    /// </summary>
    public async void StopAfterCurrentAttack()
    {
        if (attackTask == null)
        {
            Debug.Log("Attack task is null");
            return;
        }

        // Set to stop after attack regardless of task status.
        isSetToStopAfterAttack = true;

        // Await only if the task hasn't already completed.
        if (attackTask.Status != TaskStatus.RanToCompletion)
        {
            await attackTask;
        }

        isSetToStopAfterAttack = false;
        StopAttackInstantly();
        AfterLastAttack?.Invoke();
    }

    /// <summary>
    /// for players
    /// </summary>
    public void StopAttackInstantly()
    {
        if (!isAttacking) return;
        isAttackStopped = true;
    }

    private async Task AttackOnce()
    {
        attackTask = Attack();
        await attackTask;
    }

    private void RotateToTarget(GameObject target)
    {
        Transform targetTransform = target.transform;
        Vector3 lookPos = new Vector3(targetTransform.position.x, transform.position.y, targetTransform.position.z);
        if (controller.unitType != UnitType.Building)
        {
            transform.LookAt(lookPos);
        }
    }
    private async Task Attack()
    {
        // Return immediately if already attacking
        if (isCurrentlyAttacking) return;

        isCurrentlyAttacking = true;

        Extensions.GetAttackTimes(controller.attackSpeed, animAttackPoint
            , out int msBeforeAttack, out int msAfterAttack);
        GameObject target = controller.TargetController.Target;

        RotateToTarget(target);
        OnAttackStart();
        await Task.Delay(msBeforeAttack);
        if (!IsAutoAttackingAvailable())
        {
            OnAttackCancelled?.Invoke();
            isCurrentlyAttacking = false; // Ensure this is set to false here too
            return;
        }
        RotateToTarget(target);
        OnAttackImpact();
        await Task.Delay(msAfterAttack);
        OnAttackEnd();

        isCurrentlyAttacking = false;
    }

    protected virtual bool IsAutoAttackingAvailable()
    {
        if(attackBlockCount > 0) return false;
        if(isAttackStopped) return false;
        if(controller.TargetController.Target == null) return false;
        if (controller.Health.IsDead) return false;
        return true;
    }
    protected abstract void OnAttackStart();
    /// <summary>
    /// For range : projectile spawn moment, for melee : hit moment.
    /// </summary>
    protected virtual void OnAttackImpact()
    {
        if (!IsAutoAttackingAvailable()) return;
        OnActualAttackMoment?.Invoke();
    }
    protected abstract void OnAttackEnd();
}
