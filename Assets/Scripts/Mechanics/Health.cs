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
    [SerializeField] private Image healthBar;
    public int ExpToGain;

    [SyncVar(hook = nameof(UpdateHealthBar))] protected int currentHealth;

    private HeroBaseStatsData _heroStats;
    private int baseHp;
    private UnitController controller;

    private void Awake()
    {
        _heroStats = GetComponent<PlayerDataHolder>().HeroStatsData;
        baseHp = _heroStats.Hp;
        controller = GetComponent<UnitController>();
        //baseHp = 10000000;
    }
    #region Server
    public override void OnStartServer()//perfect start for pool object
    {
        StartCoroutine(StartRoutine());
    }
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

    private IEnumerator StartRoutine()
    {
        yield return new WaitUntil(() => NetworkClient.ready);
        IsDead = false;
        currentHealth = baseHp;
        if (controller.unitType != UnitType.Player)
            UnitManager.Instance.RegisterUnit(new NetworkIdentityReference(gameObject.GetComponent<NetworkIdentity>()), controller.unitType);
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
        IsDead = true;
        UnitManager.Instance.UnregisterUnits(new NetworkIdentityReference(gameObject.GetComponent<NetworkIdentity>()), controller.unitType);
        if(damageDealerTransform.TryGetComponent(out PlayerLevelController levelController))
        {
            levelController.GainExperience(ExpToGain);
        }
        ObjectPooler.Instance.CmdReturnToPool(gameObject.GetComponent<NetworkIdentity>().netId);
        OnDeath?.Invoke();
    }
    #endregion
    #region Client
    private void UpdateHealthBar(int oldHealth, int newHeatlh)
    {
        healthBar.fillAmount = (float)((float)newHeatlh / (float)baseHp);
    }
    #endregion


}

