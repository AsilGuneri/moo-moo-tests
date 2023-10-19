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
    public int CurrentHealthPercentage { get { return (currentHealth / maxHealth) * 100; } }

    private HealthBar healthBar;//
    public int ExpToGain;

    private int maxHealth;
    private int maxMana;
    private UnitController controller;
    private bool isActive = false;

    [SyncVar(hook = nameof(OnHealthChanged))]
    protected int currentHealth;
    [SyncVar(hook = nameof(OnManaChanged))]
    protected int currentMana;

    private void OnHealthChanged(int oldHealth, int newHealth)
    {
        healthBar.UpdateHealthBar(maxHealth, newHealth);
    }
    private void OnManaChanged(int oldMana, int newMana)
    {
        healthBar.UpdateMana(maxMana, newMana);
    }


    private void Awake()
    {
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
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    [Command(requiresAuthority = false)] //no authority because we dont own enemies
    public void CmdSetupHealth(int maxHealth, int maxMana)
    {
        SetupHealth(maxHealth, maxMana);
    }

    [Server]
    private void SetupHealth(int maxHealth, int maxMana)
    {
        UnitManager.Instance.RegisterUnit(controller);
        IsDead = false;
        this.maxHealth = maxHealth;
        this.maxMana = maxMana;
        currentHealth = maxHealth;
        currentMana = maxMana;
        isActive = true;
        RpcSetupHealth();
        if (maxMana > 0) RpcSetupMana();
    }
    [ClientRpc]
    private void RpcSetupHealth()
    {
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
    }
    [ClientRpc]
    private void RpcSetupMana()
    {
        healthBar.UpdateMana(maxMana, currentMana);
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
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
    }

}

