using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fire", menuName = "Scriptable Objects/Status Effects/Fire")]

public class Fire : StatusAction
{
    public override void Apply(UnitController target, Status status)
    {
    }

    public override void OnInterval(UnitController target, Status status)
    {
        Debug.Log("asilxx " + target.name + " " + status.DamagePerSec);
        target.Health.TakeDamage(status.DamagePerSec, status.Caster);
    }

    public override void Remove(UnitController target, Status status)
    {
    }
}
