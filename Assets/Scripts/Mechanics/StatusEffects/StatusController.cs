using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    List<Status> activeStatusEffects = new List<Status>();


    private void Update()
    {
        for (int i = 0; i < activeStatusEffects.Count; i++)
        {
            var status = activeStatusEffects[i];
            status.Time -= Time.deltaTime;
            if (status.Time <= 0)
            {
                RemoveStatus(status);
            }
        }
    }

    public void ApplyStatus(StatusType type, float time)
    {
        var status = new Status(type, time);
        status.Apply(this);
        activeStatusEffects.Add(status);
    }
    void RemoveStatus(Status activeStatus)
    {
        activeStatus.Remove(this);
        activeStatusEffects.Remove(activeStatus);
    }
    Status GetActiveStatus(StatusType type)
    {
        Status activeStatus = null;
        foreach (var status in activeStatusEffects)
            if (status.Type == type) activeStatus = status;
        return activeStatus;
    }


}
[Serializable]
public class Status
{
    public StatusType Type;

    public float Time;
    public StatusData Data { get => StatusManager.Instance.GetStatusData(Type); }
    public Status (StatusType type, float time)
    {
        Type = type;
        Time = time;
    }
    public void Apply(StatusController c)
    {
        Data.Apply(c);
    }
    public void Remove(StatusController c)
    {
        Data.Remove(c);
    }


}
public enum StatusType
{
    None,
    Slow,
    Stun,
    Fire,
    Poison
}