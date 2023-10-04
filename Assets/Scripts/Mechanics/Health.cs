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
    public int CurrentHealth { get => currentHealth; }
    public int CurrentHealthPercentage { get { return (currentHealth / baseHp) * 100; } }

    [SerializeField] private HealthBar healthBar;
    public int ExpToGain;

    [SyncVar(hook = nameof(OnCurrentHealthChanged))]
    protected int currentHealth;

    private void OnCurrentHealthChanged(int oldHealth, int newHealth)
    {
        healthBar.UpdateHealthBar(baseHp, newHealth);
    }

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



    [Server]
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > baseHp)
        {
            currentHealth = baseHp;
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
        currentHealth = baseHp;
        isActive = true;
        RpcSetupHealth();
    }
    [ClientRpc]
    private void RpcSetupHealth()
    {
        healthBar.UpdateHealthBar(baseHp, baseHp);
    }

    [ClientRpc]
    private void RpcUpdateHealth()
    {
        healthBar.UpdateHealthBar(baseHp, baseHp);
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
        healthBar.UpdateHealthBar(baseHp, currentHealth);
    }

}

