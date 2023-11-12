using UnityEngine;
namespace Demo.FSM.Graph
{
    public class BaseStateMachineGraph : BaseStateMachine
    {
        [SerializeField] private FSMGraph _graph;
        public new BaseStateNode CurrentState;
        public override void Init()
        {
            CurrentState = _graph.InitialState;
        }
        public override void Execute()
        {
            ((StateNode)CurrentState).Execute(this);
        }
    }
}