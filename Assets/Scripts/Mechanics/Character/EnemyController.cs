using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : UnitController
{
    protected override void Start()
    {
        base.Start();
        SubscribeAnimEvents();
    }
    public void StartAttacking(GameObject target)
    {
        movement.ClientStop();
        targetController.SetTarget(target);
        attackController.StartAutoAttack(target, attackSpeed, animAttackPoint);
    }
    public void StopAttacking()
    {
        attackController.StopAttack();
    }
    public void ChangeTarget(GameObject target)
    {
        StopAttacking();
        StartAttacking(target);
    }
}
