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

    public readonly SyncList<GameObject> Buildings = new SyncList<GameObject>();

    
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
                //unitObj.GetComponent<EnemyBrain>().StartBrain();
                break;
            case UnitType.Building:
                if (Buildings.Contains(unitObj)) return;
                Buildings.Add(unitObj);
                //unit.Value.GetComponent<EnemyBrain>().StartBrain();
                break;
        }
    }
    [ServerCallback]
    public void UnregisterUnits(UnitController unit)
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
            case UnitType.Building:
                Buildings.Remove(unitObj);
                break;
        }
    }
    public void GiveCommand(string commandPackName, MinionType minionType)
    {
        foreach (var enemy in WaveEnemies)
        {
            if (enemy.TryGetComponent(out EnemyController enemyController) && enemyController.MinionType == minionType)
            {
                enemy.GetComponent<EnemyBrain>().SetPackRoutine(commandPackName);
            }
        }
    }

    public GameObject GetClosestBuilding(Vector3 myPos)
    {
        float minDistance = float.MaxValue;
        GameObject closestBuilding = null;
        foreach (var building in Buildings)
        {
            var distance = Extensions.GetDistance(building.transform.position, myPos);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestBuilding = building;
            }
        }
        return closestBuilding;
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
    public List<GameObject> GetEnemiesInRadius(UnitController controller, float radius)
    {
        List<GameObject> units = new List<GameObject>();
        foreach (var enemyType in controller.enemyList)
        {
            foreach (var unit in GetUnitList(enemyType))
            {
                if (!unit) continue;

                bool inRange = Extensions.CheckRange(controller.transform.position, unit.transform.position, radius);
                if (inRange && !units.Contains(unit.gameObject))
                {
                    units.Add(unit.gameObject);
                }
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
        foreach (var player in Players)
        {
            if (player.GetComponent<UnitController>().isOwned) return player.GetComponent<UnitController>();
        }
        return null;
    }
    private SyncList<GameObject> GetUnitList(UnitType type)
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