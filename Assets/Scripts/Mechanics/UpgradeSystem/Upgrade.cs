using DuloGames.UI;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UpgradeData : ScriptableObject
{
    public string DisplayName;
    public string Description;
    public Sprite Icon;
    public List<Upgrade> Upgrades = new List<Upgrade>();


    public virtual void OnAcquire(StatController statController)
    {
        foreach(Upgrade upgrade in Upgrades)
        {
            upgrade.OnAcquire(statController);
        }
    }
}
[Serializable]
public class Upgrade
{
    public UpgradeType Type;
    public int Amount;
    [Range(0f, 1f)]
    public float Percentage;

    public void OnAcquire(StatController statController)
    {
        switch(Type)
        {
            case UpgradeType.Health:
                int value = 0;
                if (Amount != 0) value += Amount;
                if (Percentage != 0) value += Mathf.CeilToInt(value * Percentage);
                statController.ChangeMaxStats(value, 0);
                break;
            case UpgradeType.AttackSpeed:

                break;
            
        }
    }
}
public enum UpgradeType
{
    None,
    Health,
    AttackSpeed
}