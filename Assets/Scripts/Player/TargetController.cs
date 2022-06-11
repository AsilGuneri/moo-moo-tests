using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TargetController : NetworkBehaviour
{
    [SyncVar] public GameObject Target;
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
    }
    //USE HOOK HERE


}
