using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stun", menuName = "Scriptable Objects/Status Effects/Stun")]

public class Stun : StatusAction
{
    public override void Apply(UnitController target, Status status)
    {
        target.Movement.BlockMovement();
        target.AttackController.BlockAttacking();        
    }

    public override void OnInterval(UnitController target, Status status)
    {

    }

    public override void Remove(UnitController target, Status status)
    {
        target.Movement.RemoveMovementBlock();
        target.AttackController.RemoveAttackingBlock();
    }
}
