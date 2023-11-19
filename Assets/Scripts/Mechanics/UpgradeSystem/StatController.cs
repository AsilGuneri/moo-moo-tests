using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatController : NetworkBehaviour
{
    public int MaxHealth { get => maxHealth; }

    public HeroBaseStatsData BaseStats { get => heroBaseStats; }
    [SerializeField] private HeroBaseStatsData heroBaseStats;

    UnitController controller;

    int maxHealth;


    [SyncVar] public int AttackDamage;
    [SyncVar] public float AdditionalDamageRatio;
    [SyncVar] public float AttackSpeed;
    [SyncVar] public float AttackRange;
    [SyncVar] public int ProjectileCount = 1;


    float attackSpeedBoost = 1;
    float attackRangeBoost = 0;
    int attackDamageBoost = 0;


    private void Awake()
    {
        controller = GetComponent<UnitController>();
        
    }
    [Server]
    public void InitializeStats()
    {
        maxHealth = heroBaseStats.Health;
        controller.Health.InitializeHealth(maxHealth);
        AttackSpeed = heroBaseStats.AttackSpeed;
        AttackRange = heroBaseStats.AttackRange;
        AttackDamage = heroBaseStats.Damage;
        AdditionalDamageRatio = heroBaseStats.AdditionalDamageRatio;
    }
    [Server]
    public void ChangeMaxHealth(int additionalHealth)
    {
        maxHealth += additionalHealth;
        controller.Health.CmdUpdateMaxHealth(additionalHealth);
    }
    [Server]
    public void ChangeAttackDamage(int bonusDamage) 
    {
        attackDamageBoost += bonusDamage;
        AttackDamage = BaseStats.Damage + attackDamageBoost;
    }
    [Server]
    public void ChangeAttackSpeed(float boostRatio)
    {
        attackSpeedBoost += boostRatio;
        var newAttackSpeed = BaseStats.AttackSpeed * attackSpeedBoost;
        AttackSpeed = newAttackSpeed;
        controller.AnimationController.SetAttackSpeed(AttackSpeed);

    }
    [Server]
    public void ChangeAttackRange(float bonusRange)
    {
        attackRangeBoost += bonusRange;
        AttackRange = BaseStats.AttackRange + attackRangeBoost;
    }
    [Server]
    public void ChangeProjectileCount(int bonusCount)
    {
        ProjectileCount += bonusCount;
    }

}
