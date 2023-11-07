using UnityEngine;
namespace Demo.FSM.Graph
{
    public class BaseStateMachineGraph : BaseStateMachine
    {
        [SerializeField] private FSMGraph _graph;
        public new BaseStateNode CurrentState {
            get
            {
                return _currentState;
            }
            set 
            {
                Debug.Log("asilxx " + name);
                _currentState = value;
            }
        }
        private BaseStateNode _currentState;
        public override void Init()
        {
            CurrentState = _graph.InitialState;
            Debug.Log("asilxx " + name + " "  + CurrentState.name);
        }
        public override void Execute()
        {
            ((StateNode)CurrentState).Execute(this);
        }
    }
}