using Mono.Cecil;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline;
using UnityEngine;

[CreateAssetMenu(fileName = "PickClosestEnemy", menuName = "Scriptable Objects/EnemyBehaviours/PickClosestEnemy")]

public class PickClosestEnemy : EnemyBehaviourData
{
    public float BetterOptionCooldown;
    public float AggroRange;
    [Range(0f, 100f)]
    public float PercentageDivision = 10;
    public int HealthPercentScoreMultiplier = 1;
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
    private GameObject bestTarget;

    private float timer = 0;
    private bool onCooldown = false;


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
        AssignTargetAndExitBehavior(bestTarget);
    }

    public override void OnExit()
    {
        //onCooldown = false;
    }

    private void Update() 
    {
        if (onCooldown)
        {
            timer += Time.deltaTime;
            if(timer >= pickEnemyData.BetterOptionCooldown)
            {
                timer = 0;
                onCooldown = false;
            }
        }
    }

    private bool ShouldEnter()
    {
        if (controller.TargetController.Target == null) return true;
        if (onCooldown) return false;
        var bestTarget = GetBestTarget();
        if (bestTarget != null && bestTarget != controller.TargetController.Target)
        {
            onCooldown = true;
            this.bestTarget = bestTarget;
            return true;
        }
        return false;
    }

    private GameObject GetBestTarget()
    {
        var possibleTargets = UnitManager.Instance.GetEnemiesInRadius(controller, pickEnemyData.AggroRange);
        float bestScore = float.MinValue;
        GameObject bestTarget = null;
        foreach (var possibleTarget in possibleTargets)
        {
            var unitController = possibleTarget.GetComponent<UnitController>();
            float score = 0;

            score -= (unitController.Health.CurrentHealthPercentage % pickEnemyData.PercentageDivision);
            score -= Extensions.GetDistance(possibleTarget.transform.position, controller.transform.position);

            if (score > bestScore)
            {
                bestScore = score;
                bestTarget = possibleTarget;
            }
        }
        return bestTarget;
    }

    private void AssignTargetAndExitBehavior(GameObject closestEnemy)
    {
        if (closestEnemy == null) closestEnemy = UnitManager.Instance.GetClosestEnemy(transform.position, controller);
        controller.TargetController.SetTarget(closestEnemy);
        controller.GetComponent<EnemyBrain>().ExitBehaviour();
    }
}
