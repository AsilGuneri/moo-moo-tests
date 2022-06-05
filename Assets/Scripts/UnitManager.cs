using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

public class UnitManager : Singleton<UnitManager>
{
    public List<Health> AllAllyUnits { get; private set; } = new List<Health>();

    public void RegisterAllyUnits(Health unit)
    {
        if (AllAllyUnits.Contains(unit)) return;
        AllAllyUnits.Add(unit);
    }
    public Health GetClosestUnit(Vector3 myPosition)
    {
        float closestDistance = Mathf.Infinity;
        Health closestUnit = null;
        foreach (Health unit in AllAllyUnits)
        {
                float distance = Vector3.Distance(myPosition, unit.transform.position);
                if (closestDistance < distance) continue;
                closestDistance = distance;
                closestUnit = unit;
            
        }
        return closestUnit;
    }
}
