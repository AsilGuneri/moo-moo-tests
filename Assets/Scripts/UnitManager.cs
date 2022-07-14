using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;
using Mirror;

public class UnitManager : NetworkSingleton<UnitManager> 
{
    public SyncList<GameObject> Players { get; private set; } = new SyncList<GameObject>();
    public SyncList<GameObject> WaveEnemies { get; private set; } = new SyncList<GameObject>();


    [ServerCallback]
    public void RegisterUnit(GameObject unit, UnitType unitType)
    {
        switch (unitType)
        {
            case UnitType.Player:
                if (Players.Contains(unit)) return;
                Players.Add(unit);
                break;
            case UnitType.WaveEnemy:
                if (WaveEnemies.Contains(unit)) return;
                WaveEnemies.Add(unit);
                break;

        }
    }
    [ServerCallback]
    public void UnregisterUnits(GameObject unit, UnitType unitType)
    {
        switch (unitType)
        {
            case UnitType.Player:
                if (Players.Contains(unit)) Players.Remove(unit);
                break;
            case UnitType.WaveEnemy:
                if (WaveEnemies.Contains(unit))
                {
                    WaveEnemies.Remove(unit); 
                    if(WaveEnemies.Count <= 0) WaveManager.Instance.SpawnNextWave();

                }
                break;
        }
    }


    public GameObject GetClosestUnit(Vector3 myPosition)
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestUnit = null;
        foreach (GameObject unit in Players)
        {
                float distance = Vector3.Distance(myPosition, unit.transform.position);
                if (closestDistance < distance) continue;
                closestDistance = distance;
                closestUnit = unit;
            
        }
        return closestUnit;
    }
}
public enum UnitType
{
    Player,
    WaveEnemy
}
