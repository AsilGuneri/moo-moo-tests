using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EnemyBehaviourData : ScriptableObject
{
    public bool BlockMovement;
    public bool BlockAttacking;


    // Abstract method to create the appropriate controller
    public abstract EnemyBehaviourController CreateBehaviourController(GameObject gameObject);

    public virtual void Initialize(Transform owner)
    {
        // Create the appropriate controller
        var controller = CreateBehaviourController(owner.gameObject);
        controller.OnInitialize(this);
    }
}

public abstract class EnemyBehaviourController : MonoBehaviour
{
    public bool IsInitialized = false;
    protected EnemyBehaviourData behaviourData;
    protected UnitController unitController;

    public abstract bool EnterCondition();
    public abstract bool ExitCondition();
    public virtual void OnEnter()
    {
        if(behaviourData.BlockMovement)
        {
            unitController.Movement.BlockMovement();
        }
        if (behaviourData.BlockAttacking)
        {
            unitController.AttackController.BlockAttacking();
        }
    }
    public virtual void OnExit()
    {
        if (behaviourData.BlockMovement)
        {
            unitController.Movement.RemoveMovementBlock();
        }
        if (behaviourData.BlockAttacking)
        {
            unitController.AttackController.RemoveAttackingBlock();
        }
    }
    /// <summary>
    /// Override and keep the base of that method
    /// </summary>
    /// <param name="data"></param>
    public virtual void OnInitialize(EnemyBehaviourData data)
    {
        behaviourData = data;
    }
}