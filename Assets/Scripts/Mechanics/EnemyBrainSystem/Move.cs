using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveBehaviour", menuName = "ScriptableObjects/EnemyBehaviours/Move", order = 1)]

public class Move : EnemyBehaviourData
{
    public float ApproachDistance;

    // Create the MoveController and add it to the given game object
    public override EnemyBehaviourController CreateBehaviourController(GameObject gameObject)
    {
        var controller = gameObject.AddComponent<MoveController>();
        gameObject.GetComponent<EnemyBrain>().StateControllerDictionary.Add(State, controller);
        return controller;
    }

}
public class MoveController : EnemyBehaviourController
{
    private Move moveData;
    private UnitMovementController movement;
    private bool isIn;

    public override void OnInitialize(EnemyBehaviourData data)
    {
        base.OnInitialize(data);
        moveData = data as Move;
    }

    public override bool EnterCondition()
    {
        return IsTargetFarEnough();
    }

    public override bool ExitCondition()
    {
        return !IsTargetFarEnough();
    }

    public override void OnEnter()
    {
        isIn = true;
    }

    public override void OnExit()
    {
        isIn = false;
    }

    private void FollowTarget()
    {
        //Get the direction towards the player
        //movement.ClientMove();
    }

    private void Update()
    {
        //if (!isIn) return;
        //FollowTarget();
    }

    private bool IsTargetFarEnough()
    {
        return false;
       // return Vector2.Distance(transform.position, CombatManager.Instance.PlayerCharacter.transform.position) > moveData.ApproachDistance;
    }
}
