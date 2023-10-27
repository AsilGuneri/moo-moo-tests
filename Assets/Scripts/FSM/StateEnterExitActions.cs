using Demo.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateEnterExitActions : ScriptableObject
{
    public abstract void OnEnter(BaseStateMachine stateMachine);
    public abstract void OnExit(BaseStateMachine stateMachine);

}
