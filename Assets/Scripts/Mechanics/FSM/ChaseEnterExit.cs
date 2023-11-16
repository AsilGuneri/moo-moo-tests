using Demo.FSM;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/EnterExitActions/ChaseEnterExit")]

public class ChaseEnterExit : StateEnterExitActions
{
    public bool FollowByAttackRange;
    [ConditionalField(nameof(FollowByAttackRange), false)] public float FollowDistance;
    public override void OnEnter(BaseStateMachine stateMachine)
    {
        var controller = stateMachine.GetComponent<UnitController>();
        var target = controller.TargetController.Target.transform;
        var followRange = FollowByAttackRange ? controller.StatController.BaseStats.AttackRange : FollowDistance;
        controller.Movement.StartFollow(target, followRange);
    }

    public override void OnExit(BaseStateMachine stateMachine)
    {
        var controller = stateMachine.GetComponent<UnitController>();
        controller.Movement.StopFollow();
    }
}
