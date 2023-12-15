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


    public List<UpgradeDataLevelPair> GetRandomUpgrades(ClassType classType) //bunun içi şuanda, random tierdan random upgrade seçiyor. bunu istediğin upgrade sayısı kadar çalıştır
    {
        List<UpgradeDataLevelPair> randomUpgrades = new List<UpgradeDataLevelPair>();
        ClassSpecificUpgrades classUpgrades = GetClassUpgrades(classType);


        List<UpgradeTier> upgradeTiers = DeepCopyUpgradeTiers(classUpgrades.upgradeTiers);

        for (int i = 0; i < 4; i++)
        {
            if (upgradeTiers.Count == 0) break;
            var randomTier = Extensions.GetRandomElement(upgradeTiers);

            //var upgradeTier = upgradeTiers[randomTier];

            UpgradeDataLevelPair ownedUpgradePairInTier = UpgradeManager.Instance.acquiredUpgrades
                .Where(u => randomTier.upgradesInTier.Contains(u.data))
                .OrderByDescending(u => u.level)
                .FirstOrDefault();

            if (randomTier.OnlyOneAllowed)
            {

                if (ownedUpgradePairInTier != null)
                {
                    if (ownedUpgradePairInTier.level < ownedUpgradePairInTier.data.upgradeLevels.Count - 1)
                    {
                        randomUpgrades.Add(new UpgradeDataLevelPair(ownedUpgradePairInTier.data, ownedUpgradePairInTier.level + 1));  //bu tier onlyoneallowed ve bizde zaten bi upgrade var, onun levelı max değilse levelını arttırıp döndür.
                        randomTier.upgradesInTier.Remove(ownedUpgradePairInTier.data);
                    }
                    
                        upgradeTiers.Remove(randomTier); //bu tier onlyoneallowed ve bizde zaten bi upgrade var, onun levelı maxsa bu tier uygun değildir.
                    
                    continue;
                }
                else //bizde bu tierdan hiç upgrade yok random al 0 levelı döndür
                {
                    //int random = Random.Range(0, upgradeTiers[randomTier].upgradesInTier.Count);
                    //var randomUpgrade = upgradeTiers[randomTier].upgradesInTier[random];
                    if(randomTier.upgradesInTier.Count == 0)
                    {
                        break;
                    }
                    var randomUpgrade = Extensions.GetRandomElement(randomTier.upgradesInTier);
                    randomUpgrades.Add(new UpgradeDataLevelPair(randomUpgrade, 0));
                    randomTier.upgradesInTier.Remove(randomUpgrade);
                    continue;
                }
            }
            //else
            //{
            //    while (upgradesInTier.Count > 0) //random upgrade seçip var mı diye bakıyoruz
            //    {
            //        int random = Random.Range(0, upgradesInTier.Count);
            //        UpgradeData randomUpgrade = upgradesInTier[random];
            //        UpgradeDataLevelPair? ownedUpgrade = UpgradeManager.Instance.acquiredUpgrades.FirstOrDefault(u => u.data == randomUpgrade);

            //        if (ownedUpgrade == null) //yoksa aldığın randomu döndür
            //        {
            //            randomUpgrades.Add(new UpgradeDataLevelPair(randomUpgrade, 0));
            //        }
            //        else if (ownedUpgrade.Value.level < randomUpgrade.upgradeLevels.Count - 1) //varsa ve levelı max değilse bu upgrade i ekle, levelı +1
            //        {
            //            randomUpgrades.Add(new UpgradeDataLevelPair(randomUpgrade, ownedUpgrade.Value.level + 1));
            //        }
            //        else
            //        {
            //            upgradesInTier.Remove(randomUpgrade); //varsa ve levelı maxsa bu upgrade i listeden çıkar
            //        }

            //    }
            //}
        }

        

        return randomUpgrades;
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
