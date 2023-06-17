using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BasicAttackController : NetworkBehaviour
{
    public bool IsAttacking { get => isAttacking; }

    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform projectileSpawnPoint;

    protected UnitController controller;
    protected bool isAttacking;

    private bool isAttackStopped; //just don't set to false anywhere else, you can set to true if needed

    public Action OnStartAttack;
    public Action OnEndAttack;
    public Action OnAttackCancelled;

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


    public void StopAttack()
    {
        if (!isAttacking) return;
        isAttackStopped = true;
    }

    private async Task AttackOnce(GameObject target, float attackSpeed, float animAttackPoint)
    {
        Task attackTask = Attack(attackSpeed, animAttackPoint);
        if (attackTask.Status == TaskStatus.Running) return;
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
        if (!IsAutoAttackingAvailable()) OnAttackCancelled?.Invoke();
        OnAttackImpact();
        await Task.Delay(msAfterAttack);
        OnAttackEnd();
    }

    protected bool IsAutoAttackingAvailable()
    {
        if(isAttackStopped) return false;
        bool isAvailable = controller.TargetController.Target != null;
        return isAvailable;
    }
    protected abstract void OnAttackStart();
    protected abstract void OnAttackImpact();
    protected abstract void OnAttackEnd();
}
