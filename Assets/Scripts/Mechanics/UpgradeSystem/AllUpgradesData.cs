using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "AllUpgradesData", menuName = "Scriptable Objects/Upgrades/AllUpgradesData")]
public class AllUpgradesData : ScriptableSingleton<AllUpgradesData>
{
    [SerializeField] List<ClassSpecificUpgrades> classUpgrades;


    public List<UpgradeDataLevelPair> GetEligibleUpgrades(ClassType classType) //bunun içi şuanda, random tierdan random upgrade seçiyor. bunu istediğin upgrade sayısı kadar çalıştır
    {
       // List<UpgradeDataLevelPair> randomUpgrades = new List<UpgradeDataLevelPair>();
        ClassSpecificUpgrades classUpgrades = GetClassUpgrades(classType);

        List<UpgradeDataLevelPair> eligibleUpgrades = new List<UpgradeDataLevelPair>();

        foreach(UpgradeTier tier in classUpgrades.upgradeTiers)
        {
            if (tier.OnlyOneAllowed)
            {
                UpgradeDataLevelPair ownedUpgradePairInTier = UpgradeManager.Instance.acquiredUpgrades
                .Where(u => tier.upgradesInTier.Contains(u.data))
                .OrderByDescending(u => u.level)
                .FirstOrDefault();
                if (ownedUpgradePairInTier != null)
                {
                    if (ownedUpgradePairInTier.level < ownedUpgradePairInTier.data.upgradeLevels.Count - 1)
                    {
                        eligibleUpgrades.Add(new UpgradeDataLevelPair(ownedUpgradePairInTier.data, ownedUpgradePairInTier.level + 1));  //bu tier onlyoneallowed ve bizde zaten bi upgrade var, onun levelı max değilse levelını arttırıp döndür.
                    }
                }
                else //hiç yok tüm upgradeleri ekle 0 lv olarak 
                {
                    foreach(var upgrade in tier.upgradesInTier)
                    {
                        eligibleUpgrades.Add(new UpgradeDataLevelPair(upgrade, 0));
                    }
                }
            }
            else
            {
                foreach (var upgrade in tier.upgradesInTier)
                {
                    var highestLevelAcquiredUpgrade = UpgradeManager.Instance.acquiredUpgrades
                        .Where(u => u.data == upgrade) // filter to find the specific upgrade
                        .OrderByDescending(u => u.level) // order by level, descending
                        .FirstOrDefault(); // take the highest level (or null if not found)

                    if (highestLevelAcquiredUpgrade == null)
                    {
                        // If the upgrade is not acquired at all, it's eligible at level 0
                        eligibleUpgrades.Add(new UpgradeDataLevelPair(upgrade, 0));
                    }
                    else if (highestLevelAcquiredUpgrade.level < upgrade.upgradeLevels.Count - 1)
                    {
                        // If the upgrade is acquired but not at max level, it's eligible for the next level
                        eligibleUpgrades.Add(new UpgradeDataLevelPair(upgrade, highestLevelAcquiredUpgrade.level + 1));
                    }
                    // If the upgrade is at max level, do nothing (it's not eligible)
                }
            }

        }

        foreach(var x in eligibleUpgrades)
        {
            Debug.Log("asilxx " + x.data.name + " " + x.level);
        }

        return eligibleUpgrades;
    }

    ClassSpecificUpgrades GetClassUpgrades(ClassType classType)
    {
        ClassSpecificUpgrades classUpgrade = classUpgrades.Find(c => c.type == classType);
        if (classUpgrade == null) Debug.Log("No upgrades found for class: " + classType);
        return classUpgrades.Find(c => c.type == classType);
    }
    List<UpgradeTier> DeepCopyUpgradeTiers(List<UpgradeTier> original)
    {
        List<UpgradeTier> copy = new List<UpgradeTier>();

        foreach (var tier in original)
        {
            UpgradeTier newTier = new UpgradeTier()
            {
                TierID = tier.TierID,
                OnlyOneAllowed = tier.OnlyOneAllowed,
                upgradesInTier = new List<UpgradeData>(tier.upgradesInTier) // Assuming UpgradeData is okay to shallow copy
            };

            copy.Add(newTier);
        }

        return copy;
    }

}

[Serializable]
public class ClassSpecificUpgrades
{
    public ClassType type;
    public List<UpgradeTier> upgradeTiers;

}

[Serializable]
public class UpgradeTier
{
    public int TierID;
    public bool OnlyOneAllowed; // new field

    public List<UpgradeData> upgradesInTier;
   
}
[Serializable]
public class UpgradeDataLevelPair
{
    public UpgradeData data;
    public int level;

    public UpgradeDataLevelPair(UpgradeData data, int level)
    {
        this.data = data;
        this.level = level;
    }
}

public enum ClassType
{
    None,
    Archer
    // Add other classes as needed
}
