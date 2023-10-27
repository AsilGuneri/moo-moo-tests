using Demo.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/AttackTarget")]
public class AttackTargetAction : FSMAction
{
    public override void Execute(BaseStateMachine stateMachine)
    {
        var controller = stateMachine.GetComponent<UnitController>();
        if (controller.AttackController.IsAttacking) return;
        if (controller.Movement.IsFollowing) controller.Movement.StopFollow();
        controller.AttackController.StartAutoAttack();
        Debug.Log("AttackTarget execute");
    }
}
