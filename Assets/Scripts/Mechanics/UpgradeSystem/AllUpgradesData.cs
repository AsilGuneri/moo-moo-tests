using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllItemsData", menuName = "Scriptable Objects/Managers/AllItemsData")]
public class AllUpgradesData : ScriptableSingleton<AllUpgradesData>
{
    public List<UpgradeData> ArcherUpgrades;

    public List<UpgradeData> GetUpgrades(ClassType type)
    {
        switch (type)
        {
            case ClassType.Archer:
                return ArcherUpgrades;
        }
        return null;
    }

}


public enum ClassType
{
    None,
    Archer
}
