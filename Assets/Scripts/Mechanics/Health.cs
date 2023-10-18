using Mirror;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Health : NetworkBehaviour
{
    public Action OnDeath;
    public bool IsDead { get; private set; }
    public int CurrentMana { get => currentMana; }
    public int CurrentHealth { get => currentHealth; }
    public int CurrentHealthPercentage { get { return (currentHealth / baseHealth) * 100; } }

    private HealthBar healthBar;//
    public int ExpToGain;

    private HeroBaseStatsData _heroStats;
    private int baseHealth;
    private int baseMana;
    private UnitController controller;
    private bool isActive = false;

    [SyncVar(hook = nameof(OnHealthChanged))]
    protected int currentHealth;
    [SyncVar(hook = nameof(OnManaChanged))]
    protected int currentMana;

    private void OnHealthChanged(int oldHealth, int newHealth)
    {
        healthBar.UpdateHealthBar(baseHealth, newHealth);
    }
    private void OnManaChanged(int oldMana, int newMana)
    {
        healthBar.UpdateMana(baseMana, newMana);
    }



    private void Awake()
    {
        _heroStats = GetComponent<PlayerDataHolder>().HeroStatsData;
        baseHealth = _heroStats.Health;
        baseMana = _heroStats.Mana;
        controller = GetComponent<UnitController>();
        healthBar = GetComponent<HealthBar>();
        //baseHp = 10000000;
    }
    [Server]
    public void TakeDamage(int dmg, Transform dealerTransform)
    {
        if (IsDead) return;
        currentHealth -= dmg;
        AddDamageStats(dmg, dealerTransform);
        if (currentHealth <= 0)
        {
            if (GetComponent<UnitController>().unitType != UnitType.Player)
            {
                Die(dealerTransform);
            }
        }
    }
    [Command]
    public void CmdUseMana(int mana)
    {
        if (IsDead) return;
        currentMana -= mana;
    }


    [Server]
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > baseHealth)
        {
            currentHealth = baseHealth;
        }
    }
    [Command(requiresAuthority = false)] //no authority because we dont own enemies
    public void ResetHealth()
    {
        SetupHealth();
    }

    [Server]
    private void SetupHealth()
    {
        UnitManager.Instance.RegisterUnit(controller);
        IsDead = false;
        currentHealth = baseHealth;
        if (baseMana > 0) currentMana = baseMana; 
        isActive = true;
        RpcSetupHealth();
    }
    [ClientRpc]
    private void RpcSetupHealth()
    {
        healthBar.UpdateHealthBar(baseHealth, baseHealth);
    }

    [ClientRpc]
    private void RpcUpdateHealth()
    {
        healthBar.UpdateHealthBar(baseHealth, baseHealth);
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
        if (IsDead) return;
        IsDead = true;
        isActive = false;
        OnDeath?.Invoke();
        UnitManager.Instance.UnregisterUnits(controller);
        if (damageDealerTransform.TryGetComponent(out PlayerLevelController levelController))
        {
            levelController.GainExp(ExpToGain);
        }
        NetworkServer.UnSpawn(gameObject);
        PrefabPoolManager.Instance.PutBackInPool(gameObject);
    }


    //in case new client joined after you
    public override void OnStartClient()
    {
        base.OnStartClient();

        // Call the UpdateHealthBar method directly to make sure the client initializes with the current health values
        healthBar.UpdateHealthBar(baseHealth, currentHealth);
    }

}

