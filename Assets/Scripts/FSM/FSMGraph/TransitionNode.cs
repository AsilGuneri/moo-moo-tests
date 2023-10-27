namespace Demo.FSM.Graph
{
    [CreateNodeMenu("Transition")]
    public sealed class TransitionNode : FSMNodeBase
    {
        public Decision Decision;
        [Output] public BaseStateNode TrueState;
        [Output] public BaseStateNode FalseState;
        public void Execute(BaseStateMachineGraph stateMachine)
        {
            var trueState = GetFirst<BaseStateNode>(nameof(TrueState));
            var falseState = GetFirst<BaseStateNode>(nameof(FalseState));
            var decision = Decision.Decide(stateMachine);
            if (decision && !(trueState is RemainInStateNode))
            {
                OnStateChange(trueState, stateMachine.CurrentState, stateMachine);
                stateMachine.CurrentState = trueState;
            }
            else if (!decision && !(falseState is RemainInStateNode))
            {
                OnStateChange(falseState, stateMachine.CurrentState, stateMachine);
                stateMachine.CurrentState = falseState;
            }
        }
        private void OnStateChange(BaseStateNode newState, BaseStateNode oldState, BaseStateMachineGraph stateMachine)
        {
            if (oldState is StateNodeExtra)
            {
                var state = (StateNodeExtra)oldState;
                if (state.EnterExitActions)
                    state.OnExit(stateMachine);
            }
            if(newState is StateNodeExtra)
            {
                var state = (StateNodeExtra)newState;
                if (state.EnterExitActions)
                    state.OnEnter(stateMachine);
            }
        }
    }
}