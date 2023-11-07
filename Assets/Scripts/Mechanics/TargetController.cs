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

    [Client]
    public void SetTarget(NetworkIdentity target)
    {
        Target = target;
    }

    public bool HasTarget()
    {
        if (controller.TargetController.Target == null) return false;
        if (controller.TargetController.Target.GetComponent<Health>().IsDead) return false;
        return true;
    }
}