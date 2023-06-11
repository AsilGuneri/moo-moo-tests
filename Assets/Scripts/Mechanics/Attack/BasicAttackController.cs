using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BasicAttackController : NetworkBehaviour
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform projectileSpawnPoint;

    protected UnitController controller;

    protected GameObject currentTarget = null;


    //temp
    public int Damage;

    protected virtual void Awake()
    {
        controller = GetComponent<UnitController>();
    }

    public async void StartAutoAttack(GameObject target, float attackSpeed, float animAttackPoint)
    {
        currentTarget = target;
        while (IsAttackingAvailable())
        {
            await AttackOnce(target, attackSpeed, animAttackPoint);
        }
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
        Extensions.GetAttackTimes(attackSpeed, animAttackPoint
            , out int msBeforeAttack, out int msAfterAttack);

        Debug.Log($"asilxx {msBeforeAttack} /{msAfterAttack}");
        OnAttackStart();
        await Task.Delay(msBeforeAttack);
        OnAttackImpact();
        await Task.Delay(msAfterAttack);
        OnAttackEnd();
    }

    protected bool IsAttackingAvailable()
    {
        bool isAvailable = controller.TargetController.Target == currentTarget 
            && controller.TargetController.Target != null;
        if (!isAvailable) currentTarget = controller.TargetController.Target;
        return isAvailable;
    }
    protected abstract void OnAttackStart();
    protected abstract void OnAttackImpact();
    protected abstract void OnAttackEnd();
}
