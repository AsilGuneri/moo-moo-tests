using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

public class UnitManager : Singleton<UnitManager>
{
    public List<HealthController> AllAllyUnits { get; private set; } = new List<HealthController>();

    public void RegisterAllyUnits(HealthController unit)
    {
        if (AllAllyUnits.Contains(unit)) return;
        AllAllyUnits.Add(unit);
    }
    public HealthController GetClosestUnit(Vector3 myPosition)
    {
        float closestDistance = Mathf.Infinity;
        HealthController closestUnit = null;
        foreach (HealthController unit in AllAllyUnits)
        {
                float distance = Vector3.Distance(myPosition, unit.transform.position);
                if (closestDistance < distance) continue;
                closestDistance = distance;
                closestUnit = unit;
            
        }
        return closestUnit;
    }
}
