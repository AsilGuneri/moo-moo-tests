using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusManager", menuName = "Scriptable Objects/Managers/StatusManager")]
public class StatusManager : ScriptableSingleton<StatusManager>
{
    public GameObject StatusUIPrefab;

    [SerializeField] List<StatusData> allEffects = new List<StatusData>();

    public StatusData GetStatusData(StatusType type)
    {
        StatusData statusData = null;
        foreach (var status in allEffects)
            if (status.Type == type) statusData = status;
        return statusData;
    }
   
}
[Serializable]
public class StatusData 
{
    public StatusType Type;
    public Sprite IconSprite;
    public Color TimerColor;
    public int StackAmount = 1;
    public StatusAction Action;
}
public enum StackType
{
    NonStackable,
    Stackable,

}

public abstract class StatusAction : ScriptableObject
{
    public abstract void Apply(UnitController target, Status status);
    public abstract void Remove(UnitController target, Status status);
    public abstract void OnInterval(UnitController target, Status status);
}