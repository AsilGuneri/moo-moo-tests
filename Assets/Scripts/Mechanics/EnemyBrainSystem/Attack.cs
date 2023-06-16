using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackBehaviour", menuName = "ScriptableObjects/EnemyBehaviours/Attack", order = 1)]

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
    private Attack attackData;
    private EnemyController enemyController;
    public override void OnInitialize(EnemyBehaviourData data)
    {
        if (IsInitialized) return;
        base.OnInitialize(data);
        IsInitialized = true;
        attackData = data as Attack;
        enemyController = GetComponent<EnemyController>();
    }
    public override bool EnterCondition()
    {    
        return enemyController.HasEnemyInAttackRange(UnitType.Player);
    }

    public override bool ExitCondition()
    {
        return !enemyController.HasEnemyInAttackRange(UnitType.Player);
    }

    public override void OnEnter()
    {
        GameObject target = UnitManager.Instance.GetClosestUnit(transform.position, UnitType.Player);
        enemyController.StartAttacking(target);

    }

    public override void OnExit()
    {
        enemyController.StopAttacking();
    }
}