using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EmptyBehaviour", menuName = "ScriptableObjects/EnemyBehaviours/EmptyBehaviour")]

public class EmptyBehaviour : EnemyBehaviourData
{
    public override EnemyBehaviourController CreateBehaviourController(GameObject gameObject)
    {
        var controller = gameObject.AddComponent<EmptyBehaviourController>();
        gameObject.GetComponent<EnemyBrain>().StateControllerDictionary.Add(this.name, controller);
        return controller;
    }
}
public class EmptyBehaviourController : EnemyBehaviourController
{
    public override void OnInitialize(EnemyBehaviourData data)
    {
        if (IsInitialized) return;
        base.OnInitialize(data);
        IsInitialized = true;
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
       
    }

    public override void OnExit()
    {

    }
}