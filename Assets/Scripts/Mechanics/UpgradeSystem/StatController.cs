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

    public float AttackSpeed { get; private set; }
    public float AttackRange { get; private set; }
    public int ProjectileCount { get; private set; } = 1;


    float attackSpeedBoost = 1;
    float attackRangeBoost = 0;


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
    }

    public void ChangeMaxStats(int additionalHealth, int additionalMana)
    {

        maxHealth += additionalHealth;
        controller.Health.CmdUpdateMaxStats(additionalHealth);
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
