using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "OnHitUpgrade", menuName = "Scriptable Objects/Upgrades/OnHitUpgrade")]
public class OnHitUpgradeData : UpgradeData
{
    public List<OnHitUpgrade> OnHitUpgrades = new List<OnHitUpgrade>();

    public override void OnAcquire(StatController statController)
    {
        foreach(var onHitUpgrade in OnHitUpgrades)
        {
            onHitUpgrade.OnAcquire(statController);
        }
    }
}
[Serializable]
public class OnHitUpgrade
{
    public OnHitUpgradeType Type;
    public int Damage;
    public int DamageOverTime;
    public float Time;
    [Range(0f, 1f)]
    public float Percentage;

    public void OnAcquire(StatController statController)
    {
        statController.GetComponent<BasicAttackController>().AddOnHitEffect(this);
    }
    public void OnHit(UnitController target, Transform dmgDealer, int damageDealt)
    {
        switch(Type)
        {
            case OnHitUpgradeType.Electricity:
                ElectricityEffect(target);
                break;
            case OnHitUpgradeType.Fire:
                FireEffect(target, dmgDealer);
                break;
            case OnHitUpgradeType.Poison:
                PoisonEffect(target, dmgDealer);
                break;
            case OnHitUpgradeType.Ice:
                IceEffect(target);
                break;
            case OnHitUpgradeType.Vampire:
                VampireEffect(damageDealt, dmgDealer);
                break;
        }
    }
    void ElectricityEffect(UnitController target)
    {
        float random = Random.Range(0f, 1f);
        if (random <= Percentage) target.StatusEffect.ApplyStun(Time);
    }
    void FireEffect(UnitController target, Transform dmgDealer)
    {
        target.StatusEffect.ApplyDamagePerSecond(Time,DamageOverTime, dmgDealer);
    }
    void PoisonEffect(UnitController target, Transform dmgDealer)
    {
        target.StatusEffect.ApplyDamagePerSecond(Time, DamageOverTime, dmgDealer);
    }
    void IceEffect(UnitController target)
    {
        target.StatusEffect.ApplySlow(Time, Percentage);
    }
    void VampireEffect(int dmg, Transform dmgDealer)
    {
        int lifeSteal = Mathf.CeilToInt(Percentage * dmg);
        var dealerUnit = dmgDealer.GetComponent<UnitController>();
        dealerUnit.Health.Heal(lifeSteal, dmgDealer);
        Debug.Log("on hit VampireEffect");
    }
}
public enum OnHitUpgradeType
{
    None,
    Electricity,
    Fire,
    Poison,
    Ice,
    Vampire
}