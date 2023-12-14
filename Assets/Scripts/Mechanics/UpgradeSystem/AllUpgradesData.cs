using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

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
    //public UpgradeData GetRandomUpgrade(ClassType type)
    //{
    //    int randomTier = Random.Range(0, 2); 
    //    var upgrades = GetUpgrades(type, randomTier);
    //    int randomUpgradeId = Random.Range(0, upgrades.Count);
    //    var randomUpgrade = upgrades[randomUpgradeId];
    //    return randomUpgrade;
    //}

}

[Serializable]
public class ClassUpgrades
{
    public ClassType type;
    public List<UpgradeData> Tier0;
    public List<UpgradeData> Tier1;
    public List<UpgradeData> Tier2;
    public List<UpgradeData> Tier3;
    public List<UpgradeData> GetUpgrades(int tier)
    {
        switch(tier)
        {
            case 0:
                return Tier0;
            case 1:
                return Tier1;
            case 2:
                return Tier2;
            case 3:
                return Tier3;
        }
        return null;
    }
}

public enum ClassType
{
    None,
    Archer
}
