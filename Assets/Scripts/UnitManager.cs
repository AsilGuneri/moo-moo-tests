using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;
using Mirror;

public class UnitManager : NetworkSingleton<UnitManager> 
{
    public readonly SyncList<NetworkIdentityReference> Players = new SyncList<NetworkIdentityReference>();
    public readonly SyncList<NetworkIdentityReference> WaveEnemies = new SyncList<NetworkIdentityReference>();

    public readonly SyncList<NetworkIdentityReference> Buildings = new SyncList<NetworkIdentityReference>();


    [Command(requiresAuthority = false)]
    public void RegisterUnit(NetworkIdentityReference unit, UnitType unitType)
    {
        switch (unitType)
        {
            case UnitType.Player:
                if (Players.Contains(unit)) return;
                Players.Add(unit);
                var player = unit.Value.GetComponent<PlayerController>();
                player.OnRegister();
                break;
            case UnitType.WaveEnemy:
                if (WaveEnemies.Contains(unit)) return;
                WaveEnemies.Add(unit);
                unit.Value.GetComponent<EnemyBrain>().StartBrain();
                break;
            case UnitType.Building:
                if (Buildings.Contains(unit)) return;
                Buildings.Add(unit);
                break;


        }
    }
    [ServerCallback]
    public void UnregisterUnits(NetworkIdentityReference unit, UnitType unitType)
    {
        switch (unitType)
        {
            case UnitType.Player:

                foreach(var player in Players)
                {
                    if (player.networkId == unit.networkId) { Players.Remove(player);}
                }
                break;
            case UnitType.WaveEnemy:
                foreach (var enemy in WaveEnemies)
                {
                    if (enemy.networkId == unit.networkId) 
                    { 
                        WaveEnemies.Remove(enemy);
                        if (WaveEnemies.Count <= 0) WaveManager.Instance.OnWaveEnd();
                    }
                }
                break;
        }
    }
    public void GiveCommand(string commandPackName, MinionType minionType)
    {
        foreach (var enemy in WaveEnemies)
        {
            if (enemy.Value.TryGetComponent(out EnemyController enemyController) && enemyController.MinionType == minionType)
            {
                enemy.Value.GetComponent<EnemyBrain>().SetPackRoutine(commandPackName);    
            }
        }
    }

    public GameObject GetBaseBuilding()
    {
        foreach(var x in Buildings)
        {
            return x.Value.gameObject;
        }
        return null;
    }
    public GameObject GetClosestEnemy(Vector3 myPosition, UnitController myUnitController)
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (var enemyType in myUnitController.enemyList)
        {
            GameObject closestUnitOfThisType = GetClosestUnit(myPosition, enemyType);
            if (closestUnitOfThisType == null) continue; // No units of this type found

            float distance = Extensions.Distance(myPosition, closestUnitOfThisType.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = closestUnitOfThisType;
            }
        }

        return closestEnemy;
    }

    public GameObject GetClosestUnit(Vector3 myPosition, UnitType requestedUnitType)
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestUnit = null;
        foreach (NetworkIdentityReference unit in GetUnitList(requestedUnitType))
        {
            if (!unit.Value) continue;
            if (!unit.Value.gameObject) continue;
            float distance = Extensions.Distance(myPosition, unit.Value.gameObject.transform.position);
            if (closestDistance < distance) continue;
            closestDistance = distance;
            closestUnit = unit.Value.gameObject;
        }
        return closestUnit;
    }
    public List<GameObject> GetUnitsInRadius(Vector3 centerPos, float radius, bool isEnemy = false)
    {
        List<GameObject> units = new List<GameObject>();
        foreach (NetworkIdentityReference unit in isEnemy ? WaveEnemies : Players)
        {
            if (!unit.Value) continue;
            if (!unit.Value.gameObject) continue;

            bool inRange = Extensions.CheckRange(centerPos, unit.Value.gameObject.transform.position, radius);
            if (!inRange && !units.Contains(unit.Value.gameObject))
            {
                units.Add(unit.Value.gameObject);
            }

        }
        return units;
    }
    public bool IsInRange(Transform firstUnit, Transform secondUnit, float range)
    {
        if (Vector3.Distance(firstUnit.position, secondUnit.position) > range) return false;
        else return true;
    }
    public UnitController GetPlayerController()
    {
        foreach(var player in Players)
        {
            if (player.Value.gameObject.GetComponent<UnitController>().hasAuthority) return player.Value.gameObject.GetComponent<UnitController>();
        }
        return null;
    }
    private SyncList<NetworkIdentityReference> GetUnitList(UnitType type)
    {
        switch (type)
        {
            case UnitType.Player:
                return Players;
            case UnitType.WaveEnemy:
                return WaveEnemies;
            case UnitType.Building:
                return Buildings;
        }
        Debug.LogError($"Unit list null");
        return null;
    }
}
public enum UnitType
{
    Player,
    WaveEnemy,
    Building,
}
