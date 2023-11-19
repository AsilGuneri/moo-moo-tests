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
    [Range(-1f, 1f)]
    public float Percentage;

    public void OnAcquire(StatController statController)
    {
        float value = 0;
        switch(Type)
        {
            case UpgradeType.Health:
                value = GetValue(statController.MaxHealth);
                statController.ChangeMaxHealth(Mathf.CeilToInt(value));
                break;
            case UpgradeType.AttackDamage:
                value = GetValue(statController.AttackDamage);
                statController.ChangeAttackDamage(Mathf.CeilToInt(value));
                break;
            case UpgradeType.AttackSpeed:
                if (Percentage != 0) statController.ChangeAttackSpeed(Percentage);
                break;
            case UpgradeType.AttackRange:
                value = GetValue(statController.AttackRange);
                statController.ChangeAttackRange(value);
                break;
            case UpgradeType.ProjectileCount:
                statController.ChangeProjectileCount(Amount);
                break;
        }
    }
    float GetValue(float currentValue)
    {
        float value = 0;
        if (Amount != 0) value += Amount;
        if (Percentage != 0) value += currentValue * Percentage;
        return value;
    }
}
public enum UpgradeType
{
    None,
    Health,
    AttackDamage,
    AttackSpeed,
    AttackRange,
    ProjectileCount
}