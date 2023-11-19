using Mirror;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : NetworkBehaviour
{
    public Action<Transform> OnDeathClient;
    public Action<Transform> OnDeathServer;
    public bool IsDead { get; private set; }
    public int CurrentHealth { get => currentHealth; }
    public int CurrentHealthPercentage { get { return (currentHealth / maxHealth) * 100; } }

    private HealthBar healthBar;//
    public int ExpToGain;

    [SyncVar]
    private int maxHealth;

    private UnitController controller;

    [SyncVar(hook = nameof(OnHealthChanged))]
    protected int currentHealth;


    private void OnHealthChanged(int oldHealth, int newHealth)
    {
        healthBar.UpdateHealthBar(maxHealth, newHealth);
    }

    private void Awake()
    {
        controller = GetComponent<UnitController>();
        healthBar = GetComponent<HealthBar>();
    }
    [Server]
    public void TakeDamage(int dmg, Transform dealerTransform)
    {
        if (IsDead) return;
        currentHealth -= dmg;
        if (currentHealth <= 0) Die(dealerTransform);
    }
 
    [Server]
    public void UpdateMaxHealth(int additionalHealth)
    {
        if (additionalHealth != 0)
        {
            maxHealth += additionalHealth;
            currentHealth += additionalHealth;
            RpcUpdateHealthBar();
        }   
    }

    [Server]
    public void ResetHealth(int maxHealth)
    {
        IsDead = false;
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        RpcUpdateHealthBar();
    }
    [ClientRpc]
    private void RpcUpdateHealthBar()
    {
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
    }

    [Server]
    public void Die(Transform damageDealerTransform)
    {
        if (IsDead) return;
        IsDead = true;
        OnDeathServer?.Invoke(damageDealerTransform);
        controller.OnDeath(damageDealerTransform);
        RpcDie(damageDealerTransform);
    }

    [ClientRpc] void RpcDie(Transform damageDealerTransform)
    {
        OnDeathClient?.Invoke(damageDealerTransform);

    }


    //in case new client joined after you
    public override void OnStartClient()
    {
        base.OnStartClient();

        // Call the UpdateHealthBar method directly to make sure the client initializes with the current health values
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
    }

}

