using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
   // [SerializeField] private float tickTime = 0.1f;

    public List<BehaviourPack> Packs = new List<BehaviourPack>();
    public Dictionary<string, EnemyBehaviourController> StateControllerDictionary = new();

    public EnemyBehaviourData CurrentBehaviour => currentBehaviour;

    private EnemyBehaviourData currentBehaviour = null;
    private int currentBehaviourIndex = -1; // -1 means no behaviour is active
    private bool isInitialized;
    private bool isActive;
    private BehaviourPack defaultPack;
    private BehaviourPack currentPack;

    public void KillBrain()
    {
        SetBrainActive(false);
        if (currentBehaviour != null) ExitBehaviour();
    }

    private void InitializeBrain()
    {
        foreach (var pack in Packs)
        {
            foreach (var behaviour in pack.Behaviours)
            {
                behaviour.Initialize(transform);
            }
        }
        if (Packs.Count > 0) defaultPack = Packs[0];
    }

    public void SetPackRoutine(string packName)
    {
        if (currentPack.PackName == packName) return;
        if (currentBehaviour != null)
        {
            ExitBehaviour();
        }
        foreach (var pack in Packs)
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
        //StartCoroutine(BrainCycle());
    }

    //private IEnumerator BrainCycle()
    //{
    //    while(isInitialized && isActive) 
    //    {
    //        int nextBehaviourIndex = FindNextBehaviour();
    //        if (nextBehaviourIndex != -1 && (currentBehaviourIndex == -1 || nextBehaviourIndex < currentBehaviourIndex))
    //        {
    //            if (currentBehaviourIndex != -1)
    //            {
    //                ExitBehaviour();
    //            }
    //            currentBehaviourIndex = nextBehaviourIndex;
    //            EnterBehaviour(currentPack.Behaviours[currentBehaviourIndex]);
    //        }
    //        else if (currentBehaviourIndex != -1)
    //        {
    //            CheckExit();
    //        }
    //        //yield return Extensions.GetWait(tickTime);
    //    }
        
    //}

    private void Update()
    {
        if (!isInitialized || !isActive) return;

        int nextBehaviourIndex = FindNextBehaviour();
        if (nextBehaviourIndex != -1 && (currentBehaviourIndex == -1 || nextBehaviourIndex < currentBehaviourIndex))
        {
            if (currentBehaviourIndex != -1)
            {
                ExitBehaviour();
            }
            currentBehaviourIndex = nextBehaviourIndex;
            EnterBehaviour(currentPack.Behaviours[currentBehaviourIndex]);
        }
        else if (currentBehaviourIndex != -1)
        {
            CheckExit();
        }
    }

    private void SetBrainActive(bool isActive)
    {
        this.isActive = isActive;
    }

    private void CheckExit()
    {
        if (StateControllerDictionary[CurrentBehaviour.name].ExitCondition())
        {
            ExitBehaviour();
        }
    }

    public void ExitBehaviour()
    {
        StateControllerDictionary[CurrentBehaviour.name].OnExit();
        currentBehaviour = null;
        currentBehaviourIndex = -1;
    }

    private int FindNextBehaviour()
    {
        if (currentPack == null) currentPack = defaultPack;

        for (int i = 0; i < currentPack.Behaviours.Count; i++)
        {
            if (StateControllerDictionary[currentPack.Behaviours[i].name].EnterCondition())
            {
                return i;
            }
        }
        return -1; // In case no suitable behaviour is found
    }

    private void EnterBehaviour(EnemyBehaviourData behaviour)
    {
        currentBehaviour = behaviour;
        StateControllerDictionary[behaviour.name].OnEnter();
    }
}

[System.Serializable]
public class BehaviourPack
{
    public string PackName;
    public List<EnemyBehaviourData> Behaviours;
}
