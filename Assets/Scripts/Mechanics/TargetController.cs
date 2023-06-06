using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class TargetController : NetworkBehaviour
{
    [SyncVar] public GameObject Target;

    public bool HasTarget
    {
        get => Target != null;
    }
    [Command(requiresAuthority = false)]
    public void SetTarget(GameObject target)
    {
        Target = target;
    }
}