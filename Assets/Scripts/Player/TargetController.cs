using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TargetController : NetworkBehaviour
{
    [SyncVar] public GameObject Target;

    [Command]
    public void SyncTarget(GameObject target)
    {
        Debug.Log("xxx1" + target);
        Target = target;
        Debug.Log("xxx2" + Target);

    }
    //USE HOOK HERE


}
