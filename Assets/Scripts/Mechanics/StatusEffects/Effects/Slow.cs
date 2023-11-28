using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slow", menuName = "Scriptable Objects/Status Effects/Slow")]
public class Slow : StatusData
{
    public override void Apply(UnitController controller, Status status)
    {
        controller.StatController.ChangeMoveSpeed(-status.Ratio);
    }

    public override void OnUpdate(UnitController controller, Status status)
    {
        //throw new System.NotImplementedException();
    }

    public override void Remove(UnitController controller, Status status)
    {
        controller.StatController.ChangeMoveSpeed(status.Ratio);
    }
}
