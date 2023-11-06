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

    [Client]
    public void SetTarget(NetworkIdentity target)
    {
        Target = target;
    }

    private void Update()
    {
        if (Target)
        {

        }
    }
}