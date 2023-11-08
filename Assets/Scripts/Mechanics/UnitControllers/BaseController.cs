using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : UnitController
{
    void Start()
    {
        StartUnit();
    }
    public override void OnDeath(Transform killer) //server
    {
        base.OnDeath(killer);
        RpcOnDeath(killer);
    }

    [ClientRpc]
    void RpcOnDeath(Transform killer)
    {
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V)) { Time.timeScale = 1; }
       

    }
}
