using Demo.FSM;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[CreateAssetMenu(menuName = "FSM/Decisions/CanChase")]

public class ChaseDecision : Decision
{
    public override bool Decide(BaseStateMachine state)
    {
        var controller = state.GetComponent<UnitController>();
        bool hasTarget = controller.TargetController.HasTarget();
        bool isInRange = Extensions.CheckRangeBetweenUnits(controller.transform, controller.TargetController.Target.transform, controller.attackRange);
        return hasTarget && isInRange;
    }
}
