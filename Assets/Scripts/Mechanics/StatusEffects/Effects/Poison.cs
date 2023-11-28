using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[CreateAssetMenu(fileName = "Poison", menuName = "Scriptable Objects/Status Effects/Poison")]

public class Poison : StatusAction
{
    public override void Apply(UnitController target, Status status)
    {
        
    }

    public override void OnInterval(UnitController target, Status status)
    {
        target.Health.TakeDamage(status.DamagePerSec, status.Caster);
    }

    public override void Remove(UnitController target, Status status)
    {

    }
}
