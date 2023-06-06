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
    private bool isStarted;

    private void Awake()
    {
        //CombatUIController.Instance.OnFightStart += OnBrainStart;
    }
    private void Start()
    {
        OnBrainStart();
    }

    private void OnBrainStart()
    {
        if (isStarted) return;
        InitializeBrain();
        isStarted = true;
    }
    private void Update()
    {
        if (!isStarted) return;

        if(currentBehaviour != null)
        {
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
                CurrentState = behaviour.State;
                currentBehaviour = behaviour;
                StateControllerDictionary[CurrentState].OnEnter();
                break;
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
