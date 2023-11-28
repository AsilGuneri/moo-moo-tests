using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slow", menuName = "Scriptable Objects/Status Effects/Slow")]
public class Slow : StatusAction
{
    public override void Apply(UnitController target, Status status)
    {
        target.StatController.ChangeMoveSpeed(-status.Ratio);
    }

    public override void OnInterval(UnitController target, Status status)
    {
        //throw new System.NotImplementedException();
    }

    public override void Remove(UnitController target, Status status)
    {
        target.StatController.ChangeMoveSpeed(status.Ratio);
    }
}
