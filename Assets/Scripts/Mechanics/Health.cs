using Mirror;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : NetworkBehaviour
{
    /*public UnitType unitType;
    public int TeamId
    [SerializeField] private Slider healthBar;*/

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
    public override void OnStartServer()
    {
        StartCoroutine(StartRoutine());
    }
    [Server]
    public void TakeDamage(int dmg, Transform dealerTransform)
    {
        if (currentHealth <= 0) return;
        currentHealth -= dmg;
        AddDamageStats(dmg, dealerTransform);
        if (currentHealth <= 0)
        {
            currentHealth = baseHp;
           // Die(dealerTransform);
        }
    }
    [Server]
    public void Heal(int amount)
    {
        Debug.Log("asilxx " + name);
        currentHealth += amount;
        if(currentHealth > baseHp)
        {
            currentHealth = baseHp;
        }
    }

    private IEnumerator StartRoutine()
    {
        yield return new WaitUntil(() => NetworkClient.ready);
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
        UnitManager.Instance.UnregisterUnits(new NetworkIdentityReference(gameObject.GetComponent<NetworkIdentity>()), controller.unitType);
        if(damageDealerTransform.TryGetComponent(out PlayerLevelController levelController))
        {
            levelController.GainExperience(ExpToGain);
        }
        ObjectPooler.Instance.CmdReturnToPool(gameObject);
    }
    #endregion
    #region Client
    private void UpdateHealthBar(int oldHealth, int newHeatlh)
    {
        healthBar.fillAmount = (float)((float)newHeatlh / (float)baseHp);
    }
    #endregion


}

