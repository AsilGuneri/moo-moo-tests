using Demo.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/IsInAttackRange")]

public class IsInAttackRangeDecision : Decision
{
    public override bool Decide(BaseStateMachine state)
    {
        var controller = state.GetComponent<UnitController>();
        bool isInRange = Extensions.CheckRangeBetweenUnits(controller.transform, controller.TargetController.Target.transform, controller.attackRange);
        return isInRange;
    }
}
