using Demo.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/HasTarget")]

public class HasTargetDecision : Decision
{

    public override bool Decide(BaseStateMachine state)
    {
        bool hasTarget = state.GetComponent<UnitController>().TargetController.Target != null;
        return hasTarget;
    }

   
}