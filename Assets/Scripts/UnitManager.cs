using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;
using Mirror;

public class UnitManager : NetworkSingleton<UnitManager> 
{
    public SyncList<GameObject> AllAllyUnits { get; private set; } = new SyncList<GameObject>();

    [ServerCallback]
    public void RegisterAllyUnits(GameObject unit)
    {
        if (AllAllyUnits.Contains(unit)) return;
        AllAllyUnits.Add(unit);
    }
    public GameObject GetClosestUnit(Vector3 myPosition)
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestUnit = null;
        foreach (GameObject unit in AllAllyUnits)
        {
                float distance = Vector3.Distance(myPosition, unit.transform.position);
                if (closestDistance < distance) continue;
                closestDistance = distance;
                closestUnit = unit;
            
        }
        return closestUnit;
    }
}
