using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderController : EnemyController
{
    protected override void Awake()
    {
        base.Awake();
    }
    public virtual bool DefendCommandCondition()
    {
        if (health.CurrentHealthPercentage > 80) return false;
        return true;
    }
}
