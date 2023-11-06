using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderController : EnemyController
{
    protected override void Awake()
    {
        base.Awake();
        if(minionType != MinionType.Commander) minionType = MinionType.Commander;
    }
    public virtual bool DefendCommandCondition()
    {
        if (minionType != MinionType.Commander) return false;
        if (health.CurrentHealthPercentage > 80) return false;
        return true;
    }
}
