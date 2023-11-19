using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "AllItemsData", menuName = "Scriptable Objects/Managers/AllItemsData")]
public class AllUpgradesData : ScriptableSingleton<AllUpgradesData>
{
    [SerializeField] List<ClassUpgrades> ClassUpgradeLists;

    public List<UpgradeData> GetUpgrades(ClassType type, int tier)
    {
        foreach(ClassUpgrades upgradeList in ClassUpgradeLists)
        {
            if (upgradeList.type == type) return upgradeList.GetUpgrades(tier);
        }
        return null;
    }

}

[Serializable]
public class ClassUpgrades
{
    public ClassType type;
    public List<UpgradeData> Tier1;
    public List<UpgradeData> Tier2;
    public List<UpgradeData> Tier3;
    public List<UpgradeData> Tier4;
    public List<UpgradeData> GetUpgrades(int tier)
    {
        switch(tier)
        {
            case 1:
                return Tier1;
            case 2:
                return Tier2;
            case 3:
                return Tier3;
            case 4:
                return Tier4;
        }
        return null;
    }
}

public enum ClassType
{
    None,
    Archer
}
