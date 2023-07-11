using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PickClosestEnemy", menuName = "ScriptableObjects/EnemyBehaviours/PickClosestEnemy")]

public class PickClosestEnemy : EnemyBehaviourData
{
  
    // Create the MoveController and add it to the given game object
    public override EnemyBehaviourController CreateBehaviourController(GameObject gameObject)
    {
        var controller = gameObject.AddComponent<PickClosestEnemyController>();
        gameObject.GetComponent<EnemyBrain>().StateControllerDictionary.Add(this.name, controller);
        return controller;
    }

}
public class PickClosestEnemyController : EnemyBehaviourController
{
    private PickClosestEnemy pickEnemyData;
    private UnitController controller;

    public override void OnInitialize(EnemyBehaviourData data)
    {
        base.OnInitialize(data);
        pickEnemyData = data as PickClosestEnemy;
        controller = GetComponent<UnitController>();
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
        var closestEnemy = UnitManager.Instance.GetClosestEnemy(transform.position, controller);
        controller.TargetController.SetTarget(closestEnemy);
    }

    public override void OnExit()
    {

    }

    private bool ShouldEnter()
    {
        if (controller.TargetController.Target != null) return false;
        return true;
    }
}
