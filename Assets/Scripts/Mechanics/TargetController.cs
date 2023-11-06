using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using MyBox;
using UnityEditorInternal;

public class TargetController : NetworkBehaviour
{
    [SyncVar]
    public NetworkIdentity Target;

    UnitController controller;

    private void Awake()
    {
        controller = GetComponent<UnitController>();
    }

    //private void Update()
    //{
    //    if(Target && !IsTargetValid())
    //    {

    //    }
    //}
    //private bool IsTargetValid()
    //{
    //    if(Target.TryGetComponent(out UnitController unit))
    //    {
    //        return !unit.Health.IsDead;
    //    }
    //    return false;
    //}

    [Client]
    public void SetTarget(NetworkIdentity target)
    {
        if (Target) //old target
        {
            Target.GetComponent<Health>().OnDeathServer -= SetToNull;
        }
        Target = target;
        if (target)
        {
            target.GetComponent<Health>().OnDeathServer += SetToNull;
        }
    }
    private void SetToNull(Transform damageDealer)
    {
        if (Target == null) return;
        if (Target.TryGetComponent(out Health health)) health.OnDeathServer -= SetToNull;
        if(controller.unitType == UnitType.WaveEnemy)
        {
            var enemyController = (EnemyController)controller;
            enemyController.StateMachine.ResetMachine();
        }
        Target = null;
    }
}