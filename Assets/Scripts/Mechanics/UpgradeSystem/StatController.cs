using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatController : MonoBehaviour
{
    public int MaxHealth { get => maxHealth; }

    public HeroBaseStatsData BaseStats { get => heroBaseStats; }
    [SerializeField] private HeroBaseStatsData heroBaseStats;

    UnitController controller;

    int maxHealth;

    public int AttackDamage { get; private set; }
    public float AdditionalDamageRatio { get; private set; }
    public float AttackSpeed { get; private set; }
    public float AttackRange { get; private set; }
    public int ProjectileCount { get; private set; } = 1;


    float attackSpeedBoost = 1;
    float attackRangeBoost = 0;
    int attackDamageBoost = 0;


    private void Awake()
    {
        controller = GetComponent<UnitController>();
        
    }
    public void InitializeStats()
    {
        maxHealth = heroBaseStats.Health;
        controller.Health.CmdInitializeHealth(maxHealth);
        AttackSpeed = heroBaseStats.AttackSpeed;
        AttackRange = heroBaseStats.AttackRange;
        AttackDamage = heroBaseStats.Damage;
        AdditionalDamageRatio = heroBaseStats.AdditionalDamageRatio;
    }

    public void ChangeMaxHealth(int additionalHealth)
    {
        maxHealth += additionalHealth;
        controller.Health.CmdUpdateMaxHealth(additionalHealth);
    }
    public void ChangeAttackDamage(int bonusDamage) 
    {
        attackDamageBoost += bonusDamage;
        AttackDamage = BaseStats.Damage + attackDamageBoost;
    }
    public void ChangeAttackSpeed(float boostRatio)
    {
        attackSpeedBoost += boostRatio;
        var newAttackSpeed = BaseStats.AttackSpeed * attackSpeedBoost;
        AttackSpeed = newAttackSpeed;
        controller.AnimationController.SetAttackSpeed(AttackSpeed);

    }
    public void ChangeAttackRange(float bonusRange)
    {
        attackRangeBoost += bonusRange;
        AttackRange = BaseStats.AttackRange + attackRangeBoost;
    }
    public void ChangeProjectileCount(int bonusCount)
    {
        ProjectileCount += bonusCount;
    }

}
