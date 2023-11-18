using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;
using Mirror;

public class UnitManager : NetworkSingleton<UnitManager>
{
    public readonly SyncList<GameObject> Players = new SyncList<GameObject>();
    public readonly SyncList<GameObject> WaveEnemies = new SyncList<GameObject>();
    public readonly SyncList<GameObject> Towers = new SyncList<GameObject>();
    public readonly SyncList<GameObject> Bases = new SyncList<GameObject>();

    [Command(requiresAuthority = false)]
    public void RegisterUnit(UnitController unit)
    {
        GameObject unitObj = unit.gameObject;
        switch (unit.unitType)
        {
            case UnitType.Player:
                if (Players.Contains(unitObj)) return;
                Players.Add(unitObj);
                break;
            case UnitType.WaveEnemy:
                if (WaveEnemies.Contains(unitObj)) return;
                WaveEnemies.Add(unitObj);
                break;
            case UnitType.Base:
                if (Bases.Contains(unitObj)) return;
                Bases.Add(unitObj);
                break;
            case UnitType.Tower:
                if (Towers.Contains(unitObj)) return;
                Towers.Add(unitObj);
                break;
        }
        unit.RpcOnRegister();
    }
    [ServerCallback]
    public void RemoveUnit(UnitController unit)
    {
        var unitObj = unit.gameObject;
        switch (unit.unitType)
        {
            case UnitType.Player:
                Players.Remove(unitObj);
                break;
            case UnitType.WaveEnemy:
                WaveEnemies.Remove(unitObj);
                if (WaveEnemies.Count <= 0) WaveManager.Instance.OnWaveEnd();
                break;
            case UnitType.Base:
                Bases.Remove(unitObj);
                if (Bases.Count <= 0) GameFlowManager.Instance.SetGameState(GameState.GameEnd);
                break;
            case UnitType.Tower:
                Towers.Remove(unitObj);
                break;
        }
    }
    public List<GameObject> GetClosestEnemiesToEnemy(GameObject requestingEnemy, UnitController myUnit, int maxCount = 5, float maxDistance = Mathf.Infinity)
    {
        if (requestingEnemy == null) return new List<GameObject>();

        List<GameObject> closestEnemies = new List<GameObject>();
        Dictionary<GameObject, float> enemyDistances = new Dictionary<GameObject, float>();

        Vector3 myPosition = requestingEnemy.transform.position;

        foreach (var enemyType in myUnit.enemyList)
        {
            foreach (var unit in GetUnitList(enemyType))
            {
                if (!unit || unit == requestingEnemy) continue; // Skip self
                float distance = Extensions.Distance(myPosition, unit.transform.position);
                if (distance < maxDistance)
                {
                    enemyDistances[unit] = distance;
                }
            }
        }

        foreach (var item in enemyDistances.OrderBy(kvp => kvp.Value))
        {
            closestEnemies.Add(item.Key);
            if (closestEnemies.Count == maxCount) break;
        }

        return closestEnemies;
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
        foreach (var unit in GetUnitList(requestedUnitType))
        {
            if (!unit) continue;
            float distance = Extensions.Distance(myPosition, unit.transform.position);
            if (closestDistance < distance) continue;
            closestDistance = distance;
            closestUnit = unit;
        }
        return closestUnit;
    }

    private SyncList<GameObject> GetUnitList(UnitType type)
    {
        switch (type)
        {
            case UnitType.Player:
                return Players;
            case UnitType.WaveEnemy:
                return WaveEnemies;
            case UnitType.Base:
                return Bases;
            case UnitType.Tower:
                return Towers;
        }
        Debug.LogError($"Unit list null");
        return null;
    }
}
public enum UnitType
{
    Player,
    WaveEnemy,
    Base,
    Tower
}