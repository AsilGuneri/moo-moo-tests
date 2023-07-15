using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : UnitController
{
    public MinionType MinionType { get => minionType; }
    [SerializeField] private MinionType minionType;
    protected override void Start()
    {
        base.Start();
        SubscribeAnimEvents();
    }

}
