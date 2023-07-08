using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GroupHeal : Skill
{
    [SerializeField] private GameObject healPrefab;
    [SerializeField] private int healAmount;
    [SerializeField] private float healInterval;
    [SerializeField] private float duration;
    
    private GameObject currentHealObject;
    private bool isHealing = false;
    private float durationTimer = 0;
    private float healIntervalTimer = 0;

    public override void Use(UnitController user, UnitController target = null)
    {
        StartHeal(user, target);
    }

    private void Update()
    {
        if(isHealing)
        {
            durationTimer += Time.deltaTime;
            healIntervalTimer += Time.deltaTime;
            if (healIntervalTimer >= healInterval)
            {
                healIntervalTimer = 0;
                OnInterval();
            }
            if (durationTimer >= duration)
            {
                ResetHealObject();
            }
           

        }
    }
    protected override void OnInterrupt()
    {
        ResetHealObject();
    }
    [Command(requiresAuthority = false)]
    private void OnInterval()
    {
        var units = UnitManager.Instance.GetUnitsInRadius(currentHealObject.transform.position, 5, true);
        foreach (var unit in units)
        {
            unit.GetComponent<UnitController>().Health.Heal(healAmount);
        }
    }
    private void StartHeal(UnitController user, UnitController target)
    {
        SpawnHealObject(user, target);
        isHealing = true;
        durationTimer = 0;
        healIntervalTimer = 0;
    }
    private void ResetHealObject()
    {
        isHealing = false;
        durationTimer = 0;
        healIntervalTimer = 0;
        DestroySelf();
    }

    [Command(requiresAuthority = false)]
    private void SpawnHealObject(UnitController user, UnitController target)
    {
        var spawnPoint = transform.position + Vector3.forward * 3;
        currentHealObject = ObjectPooler.Instance.Get(healPrefab, spawnPoint, Quaternion.identity);
        

        NetworkServer.Spawn(currentHealObject);
    }
    [Server]
    private void DestroySelf()
    {
        NetworkServer.UnSpawn(currentHealObject);
        ObjectPooler.Instance.Return(currentHealObject);
    }
}