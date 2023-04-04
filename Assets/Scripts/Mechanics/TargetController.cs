﻿using System.Collections;
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
        get => _hasTarget;
        set => _hasTarget = value;
    }

    [Command]
    public void SyncTarget(GameObject target)
    {
        Target = target;
        //bi yerden client stopı çağırmıyorum
    }


}
