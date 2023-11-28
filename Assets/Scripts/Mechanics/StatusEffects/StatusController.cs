using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    StatusUIController statusUI;
    List<Status> activeStatusEffects = new List<Status>();
    UnitController controller;
    float timeSinceLastInterval;

    private void Awake()
    {
        statusUI = GetComponent<StatusUIController>();
        controller = GetComponent<UnitController>();
    }

    private void Update()
    {
        bool isInterval = false;
        timeSinceLastInterval += Time.deltaTime;
        if(timeSinceLastInterval >= 1f)
        {
            timeSinceLastInterval = 0f;
            isInterval = true;
        }
        for (int i = 0; i < activeStatusEffects.Count; i++)
        {
            var status = activeStatusEffects[i];
            if (isInterval) status.Data.Action.OnInterval(controller, status);
            status.Time -= Time.deltaTime;
            if (status.Time <= 0)
            {
                RemoveStatus(status);
            }
        }
    }

    public void ApplyStatus(StatusType type, float time, float ratio = 0, int dps = 0, Transform caster = null)
    {
        Status status = GetActiveStatus(type,out int activeOfType);
        if (status == null)
        {
            AddStatus(type, time, ratio, dps, caster);
            return;
        }
        if(status.Data.StackAmount > activeOfType)
        {
            AddStatus(type, time, ratio, dps, caster);
        }
        //else
        //{
        //    switch(status.Data.StackAmount)
        //}

        //&& status.Data.StackAmount <= activeOfType)
        //    return;
    }

    private void AddStatus(StatusType type, float time, float ratio, int dps, Transform caster)
    {
        Status newStatus = new Status(type, time, ratio, dps, caster);
        newStatus.Data.Action.Apply(controller, newStatus);
        activeStatusEffects.Add(newStatus);
        statusUI.OnStatusStart(newStatus);
    }

    void RemoveStatus(Status activeStatus)
    {
        activeStatus.Data.Action.Remove(controller, activeStatus);
        activeStatusEffects.Remove(activeStatus);
        statusUI.OnStatusEnd(activeStatus);
    }
    Status GetActiveStatus(StatusType type, out int count)
    {
        count = 0;
        Status activeStatus = null;
        foreach (var status in activeStatusEffects)
            if (status.Type == type) 
            {
                count++;
                activeStatus = status; 
            }
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
    public Transform Caster;
    public StatusData Data { get => StatusManager.Instance.GetStatusData(Type); }
    public Status (StatusType type, float time, float ratio, int damagePerSec, Transform caster)
    {
        Type = type;
        Time = time;
        Ratio = ratio;
        DamagePerSec = damagePerSec;
        Caster = caster;
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
