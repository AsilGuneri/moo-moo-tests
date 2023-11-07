using Demo.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/CanAttack")]

public class AttackDecision : Decision
{
    public override bool Decide(BaseStateMachine state)
    {
        var controller = state.GetComponent<UnitController>();
        bool hasTarget = controller.TargetController.HasTarget();
        bool isInRange = Extensions.CheckRangeBetweenUnits(controller.transform, controller.TargetController.Target.transform, controller.attackRange);
        return hasTarget && isInRange;

    }
}
