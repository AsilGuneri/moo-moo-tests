using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EnemyBehaviourData : ScriptableObject
{
    public BehaviourState State
    {
        get => state;
    }
    [SerializeField] private BehaviourState state;

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

    public abstract bool EnterCondition();
    public abstract bool ExitCondition();
    public abstract void OnEnter();
    public abstract void OnExit();
    /// <summary>
    /// Override and keep the base of that method
    /// </summary>
    /// <param name="data"></param>
    public virtual void OnInitialize(EnemyBehaviourData data)
    {
        behaviourData = data;
    }
}

public enum BehaviourState
{
    Idle,
    Move,
    Attack,
    None
}