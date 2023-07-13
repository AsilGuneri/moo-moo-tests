using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public List<EnemyBehaviourData> Behaviours = new List<EnemyBehaviourData>();

    public Dictionary<string, EnemyBehaviourController> StateControllerDictionary = new();

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
            CheckExit();
        }
        else
        {
            CheckEnter();
        }
    }
    public void SetBrainActive(bool isActive)
    {
        this.isActive = isActive;
    }
    private void CheckExit()
    {
        if (StateControllerDictionary[currentBehaviour.name].ExitCondition())
        {
            StateControllerDictionary[currentBehaviour.name].OnExit();
            currentBehaviour = null;
        }
    }
    private void CheckEnter()
    {
        foreach (var behaviour in Behaviours)
        {
            if (StateControllerDictionary[behaviour.name].EnterCondition())
            {
                currentBehaviour = behaviour;
                StateControllerDictionary[behaviour.name].OnEnter();
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
