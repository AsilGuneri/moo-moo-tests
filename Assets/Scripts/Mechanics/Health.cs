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
    [SerializeField] private UnitType unitType;
    [ConditionalField(nameof(unitType), false, UnitType.WaveEnemy)] public int ExpToGain;

    [SyncVar(hook = nameof(UpdateHealthBar))] protected int _currentHealth;

    private HeroBaseStatsData _heroStats;
    private int baseHp;

    public UnitType UnitType { get { return unitType; } }

    private void Awake()
    {
        _heroStats = GetComponent<PlayerDataHolder>().HeroStatsData;
        baseHp = _heroStats.Hp;
        //baseHp = 10000000;
    }
    #region Server
    public override void OnStartServer()
    {
        _currentHealth = baseHp;
        if(unitType != UnitType.Player) UnitManager.Instance.RegisterUnit(new NetworkIdentityReference(gameObject.GetComponent<NetworkIdentity>()), unitType);

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
        UnitManager.Instance.UnregisterUnits(new NetworkIdentityReference(gameObject.GetComponent<NetworkIdentity>()), unitType);
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

