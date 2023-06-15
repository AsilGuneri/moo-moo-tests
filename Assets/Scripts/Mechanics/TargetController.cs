using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using MyBox;

public class TargetController : NetworkBehaviour
{
    [HideInInspector][SyncVar] public GameObject Target;

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