using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : UnitController
{
    private void Start()
    {
        SubscribeAnimEvents();
    }
    public void StartAttacking(GameObject target)
    {
        movement.ClientStop();
        attackController.StartAutoAttack(target, attackSpeed, animAttackPoint);
        targetController.SetTarget(target);
    }
    public void StopAttacking()
    {
        attackController.StopAttack();
    }
    public bool HasEnemyInAttackRange(UnitType enemyType)
    {
        var target = UnitManager.Instance.GetClosestUnit(transform.position, enemyType);
        bool isInRange = Extensions.CheckRange(transform.position, target.transform.position, attackRange);
        return isInRange;
    }
}
