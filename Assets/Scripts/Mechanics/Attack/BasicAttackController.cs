using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BasicAttackController : NetworkBehaviour
{
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

    protected virtual void Awake()
    {
        controller = GetComponent<UnitController>();
    }

    public async void StartAutoAttack(GameObject target, float attackSpeed, float animAttackPoint)
    {
        if (isAttacking) return;

        isAttacking = true;
        OnStartAttack?.Invoke();

        while (IsAutoAttackingAvailable()) await AttackOnce(target, attackSpeed, animAttackPoint);

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
    }

    /// <summary>
    /// for players
    /// </summary>
    public void StopAttackInstantly()
    {
        if (!isAttacking) return;
        isAttackStopped = true;
    }

    private async Task AttackOnce(GameObject target, float attackSpeed, float animAttackPoint)
    {
        attackTask = Attack(attackSpeed, animAttackPoint);
        Transform targetTransform = target.transform;
        Vector3 lookPos = new Vector3(targetTransform.position.x, transform.position.y, targetTransform.position.z);
        transform.LookAt(lookPos);
        await attackTask;
    }
    private async Task Attack(float attackSpeed, float animAttackPoint)
    {
        Extensions.GetAttackTimes(1, attackSpeed, animAttackPoint
            , out int msBeforeAttack, out int msAfterAttack);

        OnAttackStart();
        await Task.Delay(msBeforeAttack);
        if (!IsAutoAttackingAvailable())
        {
            OnAttackCancelled?.Invoke();
            return;
        }
        OnAttackImpact();
        await Task.Delay(msAfterAttack);
        OnAttackEnd();
    }

    protected bool IsAutoAttackingAvailable()
    {
        if(attackBlockCount > 0) return false;
        if(isAttackStopped) return false;
        if(controller.TargetController.Target == null) return false;
        if (controller.Health.IsDead) return false;
        return true;
    }
    protected abstract void OnAttackStart();
    protected abstract void OnAttackImpact();
    protected abstract void OnAttackEnd();
}
