using Mirror.Examples.AdditiveLevels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatController : MonoBehaviour
{
    public int MaxHealth { get => maxHealth; }

    [SerializeField] private HeroBaseStatsData heroBaseStats;

    UnitController controller;

    int maxHealth;
    int maxMana;

    int baseHealth;
    int baseMana;
    float baseAttackSpeed;

    private void Awake()
    {
        controller = GetComponent<UnitController>();
        baseHealth = heroBaseStats.Health;
        baseMana = heroBaseStats.Mana;
        baseAttackSpeed = heroBaseStats.AttackSpeed;

        maxHealth = baseHealth;
        maxMana = baseMana;
    }
    public void InitializeStats()
    {
        controller.Health.CmdInitializeHealth(maxHealth, maxMana);
    }

    public void ChangeMaxStats(int additionalHealth, int additionalMana)
    {

        maxHealth += additionalHealth;
        maxMana += additionalMana;
        controller.Health.CmdUpdateMaxStats(additionalHealth, additionalMana);   
    }
    public void ChangeAttackSpeed(float attackSpeedFactor)
    {
        baseAttackSpeed*= attackSpeedFactor;
        controller.attackSpeed = baseAttackSpeed;
        controller.AnimationController.SetAttackSpeed(baseAttackSpeed);
        
    }
    public void Update()
    {
        if(controller.unitType!=UnitType.Player)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            ChangeAttackSpeed(0.5f);
        }
    }
}
