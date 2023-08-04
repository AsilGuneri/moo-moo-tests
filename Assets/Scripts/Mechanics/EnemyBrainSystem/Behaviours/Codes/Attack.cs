using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackBehaviour", menuName = "Scriptable Objects/EnemyBehaviours/Attack")]

public class Attack : EnemyBehaviourData
{
    public override EnemyBehaviourController CreateBehaviourController(GameObject gameObject)
    {
        var controller = gameObject.AddComponent<AttackController>();
        gameObject.GetComponent<EnemyBrain>().StateControllerDictionary.Add(this.name, controller);
        return controller;
    }
}
public class AttackController : EnemyBehaviourController
{
    private EnemyController controller;
    private BasicAttackController attackController;
    public override void OnInitialize(EnemyBehaviourData data)
    {
        if (IsInitialized) return;
        base.OnInitialize(data);
        attackController = GetComponent<BasicAttackController>();
        controller = GetComponent<EnemyController>();
        IsInitialized = true;
    }
    public override bool EnterCondition()
    {
        return ShouldEnter();
    }

    public override bool ExitCondition()
    {
        return !ShouldEnter();
    }

    public override void OnEnter()
    {
        var target = controller.TargetController.Target;
        controller.Movement.ClientStop();
        controller.TargetController.SetTarget(target);
        attackController.StartAutoAttack(target, controller.attackSpeed, controller.animAttackPoint);
    }

    public override void OnExit()
    {
        attackController.StopAfterCurrentAttack();
    }
    private bool ShouldEnter()
    {
        if (controller.TargetController.Target == null) return false;
        if (!Extensions.CheckRangeBetweenUnits(controller.transform,controller.TargetController.Target.transform, controller.attackRange))
            return false;

        return true;
        //controller.TargetController.Target.GetComponent<UnitController>().Movement.AgentRadius),
        //    controller.attackRange,controller.Movement.AgentRadius
    }
}