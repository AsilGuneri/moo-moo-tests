using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackBehaviour", menuName = "ScriptableObjects/EnemyBehaviours/Attack", order = 1)]

public class Attack : EnemyBehaviourData
{
    public override EnemyBehaviourController CreateBehaviourController(GameObject gameObject)
    {
        Debug.Log($"asilxx1 {name}");
        var controller = gameObject.AddComponent<AttackController>();
        gameObject.GetComponent<EnemyBrain>().StateControllerDictionary.Add(State, controller);
        return controller;
    }
}
public class AttackController : EnemyBehaviourController
{
    private Attack attackData;
    private TargetController targetController;
    public override void OnInitialize(EnemyBehaviourData data)
    {
        base.OnInitialize(data);
        attackData = data as Attack;
        targetController = GetComponent<TargetController>();
    }
    public override bool EnterCondition()
    {
        return true;
    }

    public override bool ExitCondition()
    {
        return false;
    }

    public override void OnEnter()
    {
        var target = UnitManager.Instance.GetClosestUnit(transform.position, UnitType.Player);
        targetController.SetTarget(target);
    }

    public override void OnExit()
    {
        targetController.SetTarget(null);
    }
}