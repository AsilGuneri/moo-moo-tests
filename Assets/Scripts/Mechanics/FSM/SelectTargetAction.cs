using Demo.FSM;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/SelectTarget")]
public class SelectTargetAction : FSMAction
{

    public override void Execute(BaseStateMachine stateMachine)
    {
        var controller = stateMachine.GetComponent<UnitController>();
        var closestEnemy = UnitManager.Instance.GetClosestEnemy(controller.transform.position, controller);
        if(closestEnemy == null) 
        {
            //should idle somehow
            return;
        }
        controller.TargetController.SetTarget(closestEnemy.GetComponent<NetworkIdentity>());

    }
}
