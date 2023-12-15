using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "AllUpgradesData", menuName = "Scriptable Objects/Upgrades/AllUpgradesData")]
public class AllUpgradesData : ScriptableSingleton<AllUpgradesData>
{
    [SerializeField] List<ClassSpecificUpgrades> classUpgrades;

    public List<UpgradeDataLevelPair> GetRandomUpgrades(ClassType classType)
    {
        var randomUpgrades = new List<UpgradeDataLevelPair>();
        var classSpecificUpgrades = GetClassUpgrades(classType);

        for (int i = 0; i < 4; i++)
        {
            var upgradeTier = GetRandomUpgradeTier(classSpecificUpgrades);
            AddRandomUpgradeFromTier(randomUpgrades, upgradeTier);
        }

        return randomUpgrades;
    }

    private UpgradeTier GetRandomUpgradeTier(ClassSpecificUpgrades classSpecificUpgrades)
    {
        int randomTierIndex = Random.Range(0, classSpecificUpgrades.upgradeTiers.Count);
        return classSpecificUpgrades.upgradeTiers[randomTierIndex];
    }

    private void AddRandomUpgradeFromTier(List<UpgradeDataLevelPair> randomUpgrades, UpgradeTier upgradeTier)
    {
        var upgradesInTier = upgradeTier.upgradesInTier.ToList();

        if (upgradeTier.OnlyOneAllowed)
        {
            AddUpgradeWithLevelValidation(randomUpgrades, upgradesInTier);
        }
        else
        {
            AddRandomUpgrade(randomUpgrades, upgradesInTier);
        }
    }

    private void AddUpgradeWithLevelValidation(List<UpgradeDataLevelPair> randomUpgrades, List<UpgradeData> upgradesInTier)
    {
        foreach (var upgrade in upgradesInTier)
        {
            if (UpgradeManager.Instance.acquiredUpgrades.TryGetValue(upgrade, out int ownedUpgradeLevel) && ownedUpgradeLevel < upgrade.upgradeLevels.Count - 1)
            {
                randomUpgrades.Add(new UpgradeDataLevelPair(upgrade, ownedUpgradeLevel + 1));
                return;
            }
        }

        AddRandomUpgrade(randomUpgrades, upgradesInTier);
    }

    private void AddRandomUpgrade(List<UpgradeDataLevelPair> randomUpgrades, List<UpgradeData> upgradesInTier)
    {
        if (upgradesInTier.Count > 0)
        {
            int randomIndex = Random.Range(0, upgradesInTier.Count);
            randomUpgrades.Add(new UpgradeDataLevelPair(upgradesInTier[randomIndex], 0));
        }
    }

    private ClassSpecificUpgrades GetClassUpgrades(ClassType classType)
    {
        var classUpgrade = classUpgrades.Find(c => c.type == classType);
        if (classUpgrade == null)
        {
            Debug.Log("No upgrades found for class: " + classType);
        }

        return classUpgrade;
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
public struct UpgradeDataLevelPair
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
