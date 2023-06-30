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

    [SyncVar(hook = nameof(UpdateHealthBar))] protected int _currentHealth;

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
        if (_currentHealth <= 0) return;
        _currentHealth -= dmg;
        AddDamageStats(dmg, dealerTransform);
        if (_currentHealth <= 0)
        {
            _currentHealth = baseHp;
           // Die(dealerTransform);
        }

    }

    private IEnumerator StartRoutine()
    {
        yield return new WaitUntil(() => NetworkClient.ready);
        _currentHealth = baseHp;
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
        NetworkServer.UnSpawn(gameObject);
        ObjectPooler.Instance.Return(gameObject);
    }
    #endregion
    #region Client
    private void UpdateHealthBar(int oldHealth, int newHeatlh)
    {
        healthBar.fillAmount = (float)((float)newHeatlh / (float)baseHp);
    }
    #endregion


}

