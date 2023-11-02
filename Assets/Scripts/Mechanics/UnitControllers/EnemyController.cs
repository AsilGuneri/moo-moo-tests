using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : UnitController
{
    public MinionType MinionType { get => minionType; }

    [SerializeField] protected MinionType minionType;

    protected override void Start()
    {
        base.Start();
        SubscribeAnimEvents();
    }

    public override void RpcOnRegister()
    {
        //throw new System.NotImplementedException();
    }
}
