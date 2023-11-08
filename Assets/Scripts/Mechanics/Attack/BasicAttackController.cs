using Mirror;
using System;
using System.Collections;
using UnityEngine;

public abstract class BasicAttackController : NetworkBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] protected float animAttackPoint;

    public Action OnEachAttackStart;
    public Action OnActualAttackMoment;
    public Action AfterLastAttack;
    public Action OnStartAttack;
    public Action OnEndAttack;
    public Action OnAttackCancelled;

    protected UnitController controller;
    private bool isCurrentlyAttacking = false;
    private bool isSetToStopAfterAttack = false;
    private bool isAttackStopped = false;
    private int attackBlockCount = 0;

    // Public properties
    public float AnimAttackPoint { get => animAttackPoint; }
    public bool IsSetToStopAfterAttack => isSetToStopAfterAttack;
    public bool IsAttacking => isCurrentlyAttacking;
    public int Damage;

    protected virtual void Awake()
    {
        controller = GetComponent<UnitController>();
    }

    public void StartAutoAttack()
    {
        if (isCurrentlyAttacking) return;
        StartCoroutine(AutoAttackRoutine());
    }
    public void StopAfterCurrentAttack()
    {
        isSetToStopAfterAttack = true;
    }
    public void StopAttackInstantly()
    {
        isAttackStopped = true;
    }

    public void BlockAttacking()
    {
        attackBlockCount++;
    }

    public void RemoveAttackingBlock()
    {
        attackBlockCount--;
    }

    private IEnumerator AutoAttackRoutine()
    {
        isCurrentlyAttacking = true;
        OnStartAttack?.Invoke();
        //yield return new WaitUntil(() => controller.TargetController.Target != null);
        while (IsAutoAttackingAvailable())
        {
            Extensions.GetAttackTimes(controller.attackSpeed, animAttackPoint,out float secondsBeforeAttack, out float secondsAfterAttack);

            RotateToTarget(controller.TargetController.Target.gameObject);

            OnEachAttackStart?.Invoke();
            OnAttackStart();
            yield return Extensions.GetWait(secondsBeforeAttack);

            if (!IsAutoAttackingAvailable())
            {
                OnAttackCancelled?.Invoke();
                continue; // Move to the next iteration without executing the rest of the loop body
            }

            RotateToTarget(controller.TargetController.Target.gameObject);
            OnAttackImpact();

            yield return Extensions.GetWait(secondsAfterAttack);
            OnAttackEnd();

            if (isSetToStopAfterAttack)
            {
                StopAttackInstantly();
                AfterLastAttack?.Invoke();
                isSetToStopAfterAttack = false;
            }
        }

        isCurrentlyAttacking = false;
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
        if (isAttackStopped) return false;
        if (!controller.TargetController.HasTarget()) return false;
        if (controller.Health.IsDead) return false;
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
    protected abstract void OnAttackEnd();


    public void ResetAttackController()
    {
        isSetToStopAfterAttack = false;
        isCurrentlyAttacking = false;

    }
}
