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

        List<int> tierList = new List<int>();
        foreach(var tier in classUpgrades.upgradeTiers)
        {
            tierList.Add(tier.TierID);
        }


        for (int i = 0; i < 4; i++)
        {
            if (tierList.Count == 0) break;
            int randomTier = Extensions.GetRandomElement(tierList);

            var upgradeTier = classUpgrades.upgradeTiers[randomTier];
            var upgradesInTier = upgradeTier.upgradesInTier.ToList();

            UpgradeDataLevelPair ownedUpgradePairInTier = UpgradeManager.Instance.acquiredUpgrades.FirstOrDefault(u => upgradesInTier.Contains(u.data));

            if (upgradeTier.OnlyOneAllowed)
            {

                if (ownedUpgradePairInTier != null)
                {
                    if (ownedUpgradePairInTier.level < ownedUpgradePairInTier.data.upgradeLevels.Count - 1)
                    {
                        randomUpgrades.Add(new UpgradeDataLevelPair(ownedUpgradePairInTier.data, ownedUpgradePairInTier.level + 1));  //bu tier onlyoneallowed ve bizde zaten bi upgrade var, onun levelı max değilse levelını arttırıp döndür.
                        upgradesInTier.Remove(ownedUpgradePairInTier.data);
                    }
                    else
                    {
                        tierList.Remove(randomTier); //bu tier onlyoneallowed ve bizde zaten bi upgrade var, onun levelı maxsa bu tier uygun değildir.
                    }
                    continue;
                }
                else //bizde bu tierdan hiç upgrade yok random al 0 levelı döndür
                {
                    int random = Random.Range(0, upgradesInTier.Count);
                    var randomUpgrade = upgradesInTier[random];
                    randomUpgrades.Add(new UpgradeDataLevelPair(randomUpgrade, 0));
                    upgradesInTier.Remove(randomUpgrade);
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
