using System.Collections.Generic;
namespace Demo.FSM.Graph
{
    [CreateNodeMenu("State")]
    public class StateNode : BaseStateNode
    {
        public List<FSMAction> Actions;
        [Output] public List<TransitionNode> Transitions;
        public void Execute(BaseStateMachineGraph baseStateMachine)
        {
            foreach (var action in Actions)
                action.Execute(baseStateMachine);
            foreach (var transition in GetAllOnPort<TransitionNode>(nameof(Transitions)))
                transition.Execute(baseStateMachine);
        }
    }
}