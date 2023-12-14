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


    public List<UpgradeData> GetRandomUpgrades(ClassType classType) //bunun içi şuanda, random tierdan random upgrade seçiyor. bunu istediğin upgrade sayısı kadar çalıştır
    {
        List<UpgradeData> randomUpgrades = new List<UpgradeData>();
        ClassSpecificUpgrades classUpgrades = GetClassUpgrades(classType);
        int randomTier = Random.Range(0, classUpgrades.upgradeTiers.Count);

        var upgradeTier = classUpgrades.upgradeTiers[randomTier];
        var upgradesInTier = upgradeTier.upgradesInTier.ToList();



        if (upgradeTier.OnlyOneAllowed)
        {
            bool isSelected = false;
            foreach(UpgradeData ownedUpgradeData in UpgradeManager.Instance.acquiredUpgrades.Keys)
            {
                UpgradeManager.Instance.acquiredUpgrades.TryGetValue(ownedUpgradeData, out int ownedUpgradeLevel);
                bool hasUpgrade = upgradesInTier.Contains(ownedUpgradeData);
                if (hasUpgrade && ownedUpgradeLevel < ownedUpgradeData.upgradeLevels.Count - 1)
                {
                    randomUpgrades.Add(ownedUpgradeData); //BU SEÇİLİRSE OLANIN LEVELINI ARTTIR
                    isSelected = true;
                }
                else upgradesInTier.Remove(ownedUpgradeData);
            }

            if (!isSelected && upgradesInTier.Count > 0) //bizde bu tierdan hiç upgrade yok
            {
                int random = Random.Range(0, upgradesInTier.Count);
                randomUpgrades.Add(upgradesInTier[random]);
            }
        }
        else
        {
            bool isSelected = false;
            while(!isSelected && upgradesInTier.Count > 0) //random upgrade seçip var mı diye bakıyoruz
            {
                int random = Random.Range(0, upgradesInTier.Count);
                UpgradeData randomUpgrade = upgradesInTier[random];
                if(!UpgradeManager.Instance.acquiredUpgrades.TryGetValue(randomUpgrade, out int ownedUpgradeLevel)) //yoksa aldığın randomu ekle
                {
                    randomUpgrades.Add(randomUpgrade);
                    isSelected = true;
                }
                else if(ownedUpgradeLevel < randomUpgrade.upgradeLevels.Count - 1) //varsa ve levelı max değilse bu upgrade i ekle, levelını aldığın yerde ayarlaman gerek.
                {
                    randomUpgrades.Add(randomUpgrade);
                    isSelected = true;
                }
                else
                {
                    upgradesInTier.Remove(randomUpgrade); //varsa ve levelı maxsa bu upgrade i listeden çıkar
                }

            }
        }
        Debug.Log(randomUpgrades[0].DisplayName);
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


public enum ClassType
{
    None,
    Archer
    // Add other classes as needed
}
