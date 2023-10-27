using Demo.FSM;
using Demo.FSM.Graph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("StateExtra")]
public class StateNodeExtra : StateNode
{
    public StateEnterExitActions EnterExitActions;
   

    public void OnEnter(BaseStateMachineGraph baseStateMachine)
    {
        EnterExitActions.OnEnter(baseStateMachine);
    }
    public void OnExit(BaseStateMachineGraph baseStateMachine)
    {
        EnterExitActions.OnExit(baseStateMachine);
    }
}
