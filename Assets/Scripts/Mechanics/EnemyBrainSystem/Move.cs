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
  //  private CharacterMovement movement;
  //  //private Transform CombatManager;
  //  private bool isIn;

    public override void OnInitialize(EnemyBehaviourData data)
    {
        base.OnInitialize(data);
        moveData = data as Move;
      //  movement = GetComponent<CharacterMovement>();
        //CombatManager = global::CombatManager.Instance.PlayerCharacter.transform;
    }

    public override bool EnterCondition()
    {
        Debug.Log("asilxx " + name);
        return IsTargetFarEnough();
    }

    public override bool ExitCondition()
    {
        return !IsTargetFarEnough();
    }

    public override void OnEnter()
    {
       // isIn = true;
    }

    public override void OnExit()
    {
       // isIn = false;
    }

    private void FollowTarget()
    {
        // Get the direction towards the player
        //Vector2 direction = (CombatManager.Instance.PlayerCharacter.transform.position - transform.position).normalized;

        // Move towards the player
       // movement.Move(direction);
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
