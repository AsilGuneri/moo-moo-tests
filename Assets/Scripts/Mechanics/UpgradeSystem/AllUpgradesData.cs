﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "AllUpgradesData", menuName = "Scriptable Objects/Upgrades/AllUpgradesData")]
public class AllUpgradesData : ScriptableSingleton<AllUpgradesData>
{
    [SerializeField] List<ClassSpecificUpgrades> classUpgrades;


    public UpgradeDataLevelPair GetRandomUpgrades(ClassType classType) //bunun içi şuanda, random tierdan random upgrade seçiyor. bunu istediğin upgrade sayısı kadar çalıştır
    {
        ClassSpecificUpgrades classUpgrades = GetClassUpgrades(classType);
        int randomTier = Random.Range(0, classUpgrades.upgradeTiers.Count);

        var upgradeTier = classUpgrades.upgradeTiers[randomTier];
        var upgradesInTier = upgradeTier.upgradesInTier.ToList();



        if (upgradeTier.OnlyOneAllowed)
        {
            foreach(UpgradeData ownedUpgradeData in UpgradeManager.Instance.acquiredUpgrades.Keys)
            {
                UpgradeManager.Instance.acquiredUpgrades.TryGetValue(ownedUpgradeData, out int ownedUpgradeLevel);
                bool hasUpgrade = upgradesInTier.Contains(ownedUpgradeData);
                if (hasUpgrade && ownedUpgradeLevel < ownedUpgradeData.upgradeLevels.Count - 1)
                {
                    return new UpgradeDataLevelPair(ownedUpgradeData, ownedUpgradeLevel + 1);  //bu tier onlyoneallowed ve bizde zaten bi upgrade var, onun levelı max değilse levelını arttırıp döndür.
                }
                else return new UpgradeDataLevelPair(null, -1); //bu tier onlyoneallowed ve bizde zaten bi upgrade var, onun levelı maxsa null döndür.
            }

            if (upgradesInTier.Count > 0) //bizde bu tierdan hiç upgrade yok random al 0 levelı döndür
            {
                int random = Random.Range(0, upgradesInTier.Count);
                return new UpgradeDataLevelPair(upgradesInTier[random], 0);
            }
        }
        else
        {
            while(upgradesInTier.Count > 0) //random upgrade seçip var mı diye bakıyoruz
            {
                int random = Random.Range(0, upgradesInTier.Count);
                UpgradeData randomUpgrade = upgradesInTier[random];
                if(!UpgradeManager.Instance.acquiredUpgrades.TryGetValue(randomUpgrade, out int ownedUpgradeLevel)) //yoksa aldığın randomu döndür
                {
                    return new UpgradeDataLevelPair(randomUpgrade, 0);
                }
                else if(ownedUpgradeLevel < randomUpgrade.upgradeLevels.Count - 1) //varsa ve levelı max değilse bu upgrade i ekle, levelını aldığın yerde ayarlaman gerek.
                {
                    return new UpgradeDataLevelPair(randomUpgrade, ownedUpgradeLevel + 1);
                }
                else
                {
                    upgradesInTier.Remove(randomUpgrade); //varsa ve levelı maxsa bu upgrade i listeden çıkar
                }

            }
        }

        return new UpgradeDataLevelPair(null, -1);
    }

    ClassSpecificUpgrades GetClassUpgrades(ClassType classType)
    {
        ClassSpecificUpgrades classUpgrade = classUpgrades.Find(c => c.type == classType);
        if (classUpgrade == null) Debug.Log("No upgrades found for class: " + classType);
        return classUpgrades.Find(c => c.type == classType);
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
