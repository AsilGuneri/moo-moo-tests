using Demo.FSM;
using MyBox;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

namespace Demo.MyFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/Chase")]
    public class ChaseAction : FSMAction
    {
        public bool FollowByAttackRange;
        [ConditionalField(nameof(FollowByAttackRange), false)] public float FollowDistance;
        public override void Execute(BaseStateMachine stateMachine)
        {
            var controller = stateMachine.GetComponent<UnitController>();
            var target = controller.TargetController.Target.transform;
            var followRange = FollowByAttackRange ? controller.attackRange : FollowDistance;
            controller.Movement.StartFollow(target, followRange);
        }
    }
}