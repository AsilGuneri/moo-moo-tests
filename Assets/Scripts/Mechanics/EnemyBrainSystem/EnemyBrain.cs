using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public BehaviourState CurrentState = BehaviourState.None; //for debug
    public List<EnemyBehaviourData> Behaviours = new List<EnemyBehaviourData>();

    public Dictionary<BehaviourState, EnemyBehaviourController> StateControllerDictionary = new();

    private EnemyBehaviourData currentBehaviour = null;
    private bool isInitialized;
    private bool isActive;

    private void OnEnable()
    {
        OnBrainStart();
        isActive = true;
    }
    private void OnDisable()
    {
        //reset if needed
        isActive = false;
    }

    public void OnBrainStart()
    {
        if (isInitialized) return;
        InitializeBrain();
        isInitialized = true;
    }
    private void Update()
    {
        if (!isInitialized || !isActive) return;

        if (currentBehaviour != null)
        {
            Debug.Log("asilxx1" + currentBehaviour.name);
            CheckExit();
        }
        else
        {
            CheckEnter();
        }
    }
    private void CheckExit()
    {
        if (StateControllerDictionary[CurrentState].ExitCondition())
        {
            StateControllerDictionary[CurrentState].OnExit();
            CurrentState = BehaviourState.None;
            currentBehaviour = null;
        }
    }
    private void CheckEnter()
    {
        foreach (var behaviour in Behaviours)
        {
            if (StateControllerDictionary[behaviour.State].EnterCondition())
            {
                Debug.Log("asilxx2 accepted is : " + behaviour.name );

                CurrentState = behaviour.State;
                currentBehaviour = behaviour;
                StateControllerDictionary[CurrentState].OnEnter();
                return;
            }
        }
    }
    private void InitializeBrain()
    {
        foreach (var behaviour in Behaviours)
        {
            behaviour.Initialize(transform);
        }
    }
}
