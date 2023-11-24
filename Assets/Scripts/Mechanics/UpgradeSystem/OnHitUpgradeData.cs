using System;
using System.Collections;
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
    public void OnHit()
    {
        switch(Type)
        {
            case OnHitUpgradeType.Electricity:
                ElectricityEffect();
                break;
            case OnHitUpgradeType.Fire:
                FireEffect();
                break;
            case OnHitUpgradeType.Poison:
                PoisonEffect();
                break;
            case OnHitUpgradeType.Ice:
                IceEffect();
                break;
            case OnHitUpgradeType.Vampire:
                VampireEffect();
                break;
        }
    }
    void ElectricityEffect()
    {
        float random = Random.Range(0f, 1f);
        if (random <= Percentage) Debug.Log("enemy stun");
    }
    void FireEffect()
    {
        Debug.Log("on hit FireEffect");
    }
    void PoisonEffect()
    {
        Debug.Log("on hit PoisonEffect");
    }
    void IceEffect()
    {
        Debug.Log("on hit IceEffect");
    }
    void VampireEffect()
    {
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