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

    private void Awake()
    {
        controller = GetComponent<UnitController>();
        baseHealth = heroBaseStats.Health;
        baseMana = heroBaseStats.Mana;

        maxHealth = baseHealth;
        maxMana = baseMana;
    }
    public void SetupStats()
    {
        controller.Health.CmdSetupHealth(maxHealth, maxMana);
    }

    //public void ChangeMaxHealth(int additionalHealth)
    //{
    //    maxHealth = additionalHealth;
    //}
}
