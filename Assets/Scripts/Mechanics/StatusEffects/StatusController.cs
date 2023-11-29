using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    StatusUIController statusUI;
    List<Status> activeStatusEffects = new();
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
        var effects = GetActiveEffectsOfType(type);
        if(effects != null && effects.Count <= 0)
        {
            var newStatus = AddStatus(type, time, ratio, dps, caster);
            statusUI.OnStatusStart(newStatus);
            return;
        }
        if (effects[0].Data.StackAmount > effects.Count)
        {
            AddStatus(type, time, ratio, dps, caster);
            statusUI.UpdateStatusUI(effects[0], effects.Count + 1);
            return;
        }
    }

    Status AddStatus(StatusType type, float time, float ratio, int dps, Transform caster)
    {
        Status newStatus = new(type, time, ratio, dps, caster);
        newStatus.Data.Action.Apply(controller, newStatus);
        activeStatusEffects.Add(newStatus);
        return newStatus;
    }
    void RemoveStatus(Status activeStatus)
    {
        activeStatus.Data.Action.Remove(controller, activeStatus);
        activeStatusEffects.Remove(activeStatus);
        var effects = GetActiveEffectsOfType(activeStatus.Type);
        statusUI.OnStatusEnd(activeStatus, effects);
    }
    List<Status> GetActiveEffectsOfType(StatusType type)
    {
        List<Status> activeStatus = new();
        foreach (var status in activeStatusEffects)
            if (status.Type == type) activeStatus.Add(status);
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
    public int Stack = 0;
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
