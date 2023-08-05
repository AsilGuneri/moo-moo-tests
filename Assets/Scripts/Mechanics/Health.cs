using Mirror;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : NetworkBehaviour
{
    public Action OnDeath;
    public bool IsDead { get; private set; }
    public int CurrentHealthPercentage { get { return (currentHealth / baseHp) * 100; } }

    [SerializeField] private HealthBar healthBar;
    public int ExpToGain;

    [SyncVar(hook = nameof(UpdateHealthBar))] protected int currentHealth;

    private HeroBaseStatsData _heroStats;
    private int baseHp;
    private UnitController controller;
    private bool isActive = false;

    private void Awake()
    {
        _heroStats = GetComponent<PlayerDataHolder>().HeroStatsData;
        baseHp = _heroStats.Hp;
        controller = GetComponent<UnitController>();
        //baseHp = 10000000;
    }
    #region Server
    
    [Server]
    public void TakeDamage(int dmg, Transform dealerTransform)
    {
        if (IsDead) return;
        currentHealth -= dmg;
        AddDamageStats(dmg, dealerTransform);
        if (currentHealth <= 0)
        {
            if(GetComponent<UnitController>().unitType != UnitType.Player) Die(dealerTransform);
        }
    }
    [Server]
    public void Heal(int amount)
    {
        currentHealth += amount;
        if(currentHealth > baseHp)
        {
            currentHealth = baseHp;
        }
    }

    public void ResetHealth()
    {
        SetupHealth();
    }
    private void SetupHealth()
    {
        IsDead = false;
        currentHealth = baseHp;
        healthBar.SetupHealthBar(currentHealth);
        if (controller.unitType != UnitType.Player)
            UnitManager.Instance.RegisterUnit(new NetworkIdentityReference(gameObject.GetComponent<NetworkIdentity>()), controller.unitType);
        isActive = true;
    }

    private void AddDamageStats(int dmg, Transform dealerTransform)
    {
        if (dealerTransform.TryGetComponent(out PlayerController playerController))
        {
            playerController.AddDamageDealt(dmg);
        }
        if (TryGetComponent(out PlayerController myPlayerController))
        {
            {
                myPlayerController.AddDamageTanked(dmg);
            }
        }
    }

    [Server]
    private void Die(Transform damageDealerTransform)
    {
        isActive = false;
        OnDeath?.Invoke();
        IsDead = true;
        UnitManager.Instance.UnregisterUnits(new NetworkIdentityReference(gameObject.GetComponent<NetworkIdentity>()), controller.unitType);
        if(damageDealerTransform.TryGetComponent(out PlayerLevelController levelController))
        {
            levelController.GainExperience(ExpToGain);
        }
        ObjectPooler.Instance.CmdReturnToPool(gameObject.GetComponent<NetworkIdentity>().netId);
    }
    #endregion
    #region Client
    private void UpdateHealthBar(int oldHealth, int newHeatlh)
    {
        if (!isActive) return;
        healthBar.UpdateHealthBar(newHeatlh);
    }
    #endregion


}

