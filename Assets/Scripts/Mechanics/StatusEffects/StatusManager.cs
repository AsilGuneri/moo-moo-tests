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

public abstract class StatusData : ScriptableObject
{
    public StatusType Type;
    public Sprite IconSprite;
    public Color TimerColor;

    public abstract void Apply(UnitController controller, Status status);
    public abstract void Remove(UnitController controller, Status status);
    public abstract void OnUpdate(UnitController controller, Status status);
}

