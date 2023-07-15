using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public List<BehaviourPack> Packs = new List<BehaviourPack>();

    public Dictionary<string, EnemyBehaviourController> StateControllerDictionary = new();

    public EnemyBehaviourData CurrentBehaviour { get => currentBehaviour; }     

    
    private EnemyBehaviourData currentBehaviour = null;
    private bool isInitialized;
    private bool isActive;
    private BehaviourPack defaultPack;
    private BehaviourPack currentPack;

    private void OnEnable()
    {
        StartBrain();
    }
    public void KillBrain()
    {
        ExitState();
        SetBrainActive(false);
    }

    private void InitializeBrain()
    {
        foreach(var pack in Packs)
        {
            foreach(var behaviour in pack.Behaviours)
            {
                behaviour.Initialize(transform);
            }
        }

        if (Packs.Count > 0) defaultPack = Packs[0];
    }
    public void SetPackRoutine(string packName)
    {
        if(CurrentBehaviour != null)
        {
            ExitState();
        }
        foreach(var pack in Packs)
        {
            if (pack.PackName == packName)
            {
                currentPack = pack;
                return;
            }
        }
    }
    public void StartBrain()
    {
        if (!isInitialized)
        {
            InitializeBrain();
            isInitialized = true;
        }
        isActive = true;
    }
    private void Update()
    {
        if (!isInitialized || !isActive) return;

        if (CurrentBehaviour != null)
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
        if (StateControllerDictionary[CurrentBehaviour.name].ExitCondition())
        {
            ExitState();
        }
    }

    private void ExitState()
    {
        StateControllerDictionary[CurrentBehaviour.name].OnExit();
        currentBehaviour = null;
    }

    private void CheckEnter()
    {
        if(currentPack.Behaviours.Count <= 0 ) currentPack = defaultPack;
        foreach (var behaviour in currentPack.Behaviours)
        {
            if (StateControllerDictionary[behaviour.name].EnterCondition())
            {
                currentBehaviour = behaviour;
                StateControllerDictionary[behaviour.name].OnEnter();
                return;
            }
        }
    }
    
}
[System.Serializable]
public class BehaviourPack
{
    public string PackName;
    public List<EnemyBehaviourData> Behaviours;
}