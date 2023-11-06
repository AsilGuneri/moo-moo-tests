using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using MyBox;

public class TargetController : NetworkBehaviour
{
    [SyncVar]
    public NetworkIdentity Target;

    UnitController controller;

    private void Awake()
    {
        controller = GetComponent<UnitController>();
    }

    private void Update()
    {
        ValidateTarget();
    }
    private void ValidateTarget()
    {
        if (!isClient) return;
        if (!Target) return;
        if (Target.gameObject.activeInHierarchy) return;

        SetTarget(null);
    }

    [Client]
    public void SetTarget(NetworkIdentity target)
    {
        Target = target;
        if (target == null && controller.unitType is UnitType.WaveEnemy)
        {
            var enemyController = controller as EnemyController;
            enemyController.StateMachine.ResetMachine();
        }
    }

   // [Command]
    //private void SetToNull(Transform damageDealer)
    //{
    //    if (Target == null) return;
    //    if (Target.TryGetComponent(out Health health)) health.OnDeathClient -= SetToNull;
    //    if(controller.unitType == UnitType.WaveEnemy)
    //    {
    //        var enemyController = (EnemyController)controller;
    //        enemyController.StateMachine.ResetMachine();
    //    }
    //    Target = null;
    //}
}