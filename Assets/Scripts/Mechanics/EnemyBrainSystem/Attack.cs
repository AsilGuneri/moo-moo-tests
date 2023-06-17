using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackBehaviour", menuName = "ScriptableObjects/EnemyBehaviours/Attack")]

public class Attack : EnemyBehaviourData
{
    public override EnemyBehaviourController CreateBehaviourController(GameObject gameObject)
    {
        var controller = gameObject.AddComponent<AttackController>();
        gameObject.GetComponent<EnemyBrain>().StateControllerDictionary.Add(State, controller);
        return controller;
    }
}
public class AttackController : EnemyBehaviourController
{
    private EnemyController controller;
    public override void OnInitialize(EnemyBehaviourData data)
    {
        if (IsInitialized) return;
        base.OnInitialize(data);
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
        controller.StartAttacking(controller.TargetController.Target);
    }

    public override void OnExit()
    {
        controller.StopAttacking();
    }
    private bool ShouldEnter()
    {
        if (controller.TargetController.Target == null) return false;
        if (!Extensions.CheckRange(controller.TargetController.Target.transform.position, 
            transform.position, controller.attackRange)) return false;

        return true;
    }
}