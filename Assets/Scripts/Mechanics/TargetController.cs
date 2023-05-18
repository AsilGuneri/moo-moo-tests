using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class TargetController : NetworkBehaviour
{
    [NonSerialized] [SyncVar] public GameObject Target;
    private bool _hasTarget;
    public bool HasTarget
    {
        get => Target != null;
    }

}