using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TowerController : UnitController
{
    protected bool isActive = false;
    private float counter = 0;


    public void StartTower()
    {
        isActive = true;
        StartCoroutine(TowerActivision());
        attackController.OnStartAttack += OnAttackStart;
        attackController.OnActualAttackMoment += OnProjectileSpawn;
        attackController.OnEndAttack += OnAttackEnd;
    }
    public void StopTower()
    {
        StopCoroutine(TowerActivision());
        isActive = false;
        attackController.OnStartAttack -= OnAttackStart;
        attackController.OnActualAttackMoment -= OnProjectileSpawn;
        attackController.OnEndAttack -= OnAttackEnd;
    }

    private void OnAttackStart()
    {

    }

    private void OnProjectileSpawn()
    {

    }
    private void OnAttackEnd()
    {

    }
    private IEnumerator TowerActivision()
    {
        counter = 0;
        float tickInterval = 0.1f;
        while(isActive)
        {
            yield return Extensions.GetWait(tickInterval);
            counter+= tickInterval;

            if (targetController.Target && !attackController.IsAttacking)
            {
                attackController.StartAutoAttack(targetController.Target, attackSpeed);
            }
            else
            {
                GameObject target = UnitManager.Instance.GetClosestEnemy(transform.position, this);
                if (target != targetController.Target)
                    targetController.SetTarget(target);
            }
        }
    }
}
