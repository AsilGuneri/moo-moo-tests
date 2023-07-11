using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using MyBox;

public class TargetController : NetworkBehaviour
{
    [SyncVar] public GameObject Target;

    [Command(requiresAuthority = false)]
    public void SetTarget(GameObject target)
    {
        Target = target;
        if (target)
        {
            target.GetComponent<Health>().OnDeath += SetToNull;
        }
    }
    private void SetToNull()
    {
        Target.GetComponent<Health>().OnDeath -= SetToNull;
        Target = null;
    }
}