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

    public virtual bool DefendCommandCondition()
    {
        if(minionType != MinionType.Commander) return false;
        Debug.Log("asilxx " + health.CurrentHealthPercentage);
        if (health.CurrentHealthPercentage > 80) return false;
        return true;
    }
}
