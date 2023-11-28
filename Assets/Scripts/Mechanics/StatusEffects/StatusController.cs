using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    StatusUIController statusUI;
    List<Status> activeStatusEffects = new List<Status>();
    UnitController controller;

    private void Awake()
    {
        statusUI = GetComponent<StatusUIController>();
        controller = GetComponent<UnitController>();
    }

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

    public void ApplyStatus(StatusType type, float time, float ratio, int dps)
    {
        var status = new Status(type, time, ratio, dps);
        status.Data.Apply(controller, status);
        activeStatusEffects.Add(status);
        statusUI.OnStatusStart(status);
    }
    void RemoveStatus(Status activeStatus)
    {
        activeStatus.Data.Remove(controller, activeStatus);
        activeStatusEffects.Remove(activeStatus);
        statusUI.OnStatusEnd(activeStatus);
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
    public float Ratio;
    public int DamagePerSec;
    public StatusData Data { get => StatusManager.Instance.GetStatusData(Type); }
    public Status (StatusType type, float time, float ratio, int damagePerSec)
    {
        Type = type;
        Time = time;
        Ratio = ratio;
        DamagePerSec = damagePerSec;
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