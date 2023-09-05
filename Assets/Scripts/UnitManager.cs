using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;
using Mirror;

public class UnitManager : NetworkSingleton<UnitManager> 
{
    List<UnitController> Players = new List<UnitController>();
    List<UnitController> WaveEnemies = new List<UnitController>();
    List<UnitController> Buildings = new List<UnitController>();

    [Command(requiresAuthority =false)]
    public void RegisterUnitClient(UnitController unit)
    {
        RegisterUnitServer(unit);
    }
    [ServerCallback]
    public void RegisterUnitServer(UnitController unit)
    {
        var unitType = unit.unitType;
        switch (unitType)
        {
            case UnitType.Player:
                var playerUnit = unit as PlayerController;
                if (Players.Contains(playerUnit)) return;
                Players.Add(playerUnit);
                break;
            case UnitType.WaveEnemy:
                var enemyUnit = unit as EnemyController;
                if (WaveEnemies.Contains(enemyUnit)) return;
                WaveEnemies.Add(enemyUnit);
                enemyUnit.GetComponent<EnemyBrain>().StartBrain();
                break;
            case UnitType.Building:
                var towerUnit = unit as TowerController;
                if (Buildings.Contains(towerUnit)) return;
                Buildings.Add(towerUnit);
                break;
        }
    }
    [ServerCallback]
    public void UnregisterUnits(NetworkIdentityReference unit, UnitType unitType)
    {
        //switch (unitType)
        //{
        //    case UnitType.Player:

        //        foreach(var player in Players)
        //        {
        //            if (player.networkId == unit.networkId) { Players.Remove(player);}
        //        }
        //        break;
        //    case UnitType.WaveEnemy:
        //        foreach (var enemy in WaveEnemies)
        //        {
        //            if (enemy.networkId == unit.networkId) 
        //            { 
        //                WaveEnemies.Remove(enemy);
        //                if (WaveEnemies.Count <= 0) WaveManager.Instance.OnWaveEnd();
        //            }
        //        }
        //        break;
        //    case UnitType.Building:
        //        foreach(var building in Buildings)
        //        {
        //            Buildings.Remove(building);
        //        }
        //        break;
        //}
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
        foreach(var building in Buildings)
        {
            var distance = Extensions.GetDistance(building.transform.position, myPos);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestBuilding = building.gameObject;
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
        foreach (UnitController unit in GetUnitList(requestedUnitType))
        {
            if (!unit) continue;
            if (!unit.gameObject) continue;
            float distance = Extensions.Distance(myPosition, unit.gameObject.transform.position);
            if (closestDistance < distance) continue;
            closestDistance = distance;
            closestUnit = unit.gameObject;
        }
        return closestUnit;
    }
    public List<GameObject> GetEnemiesInRadius(UnitController controller, float radius)
    {
        List<GameObject> units = new List<GameObject>();
        foreach(var enemyType in controller.enemyList)
        {
            foreach (var unit in GetUnitList(enemyType))
            {
                if (!unit) continue;
                if (!unit.gameObject) continue;

                bool inRange = Extensions.CheckRange(controller.transform.position, unit.gameObject.transform.position, radius);
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
        foreach(var player in Players)
        {
            if (player.gameObject.GetComponent<UnitController>().isOwned) return player.gameObject.GetComponent<UnitController>();
        }
        return null;
    }
    private List<UnitController> GetUnitList(UnitType type)
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
