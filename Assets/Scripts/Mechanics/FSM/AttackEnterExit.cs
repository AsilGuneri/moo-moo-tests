using Demo.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/EnterExitActions/AttackEnterExit", fileName = "AttackEnterExit")]
public class AttackEnterExit : StateEnterExitActions
{
    public override void OnEnter(BaseStateMachine stateMachine)
    {
        var controller = stateMachine.GetComponent<UnitController>();
        controller.AttackController.StartAutoAttack();
    }

    public override void OnExit(BaseStateMachine stateMachine)
    {
        var controller = stateMachine.GetComponent<UnitController>();
        controller.AttackController.StopAfterCurrentAttack();
    }
}
