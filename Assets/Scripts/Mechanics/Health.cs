using Mirror;
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

    [SerializeField] private Slider healthBar;
    [SerializeField] private UnitType unitType;

    [SyncVar(hook = nameof(UpdateHealthBar))] protected int _currentHealth;

    private HeroBaseStatsData _heroStats;
    private int baseHp;

    public event Action ServerOnDeath;

    private void Awake()
    {
        _heroStats = GetComponent<PlayerDataHolder>().HeroStatsData;
        baseHp = _heroStats.Hp;
    }
    #region Server
    public override void OnStartServer()
    {
        _currentHealth = baseHp;
        if(unitType != UnitType.Player) UnitManager.Instance.RegisterUnit(new NetworkIdentityReference(gameObject.GetComponent<NetworkIdentity>()), unitType);

    }
    [Server]
    public void TakeDamage(int dmg)
    {
        if (_currentHealth <= 0) return;
        _currentHealth -= dmg;
        if (_currentHealth <= 0)
        {
            ServerOnDeath?.Invoke();
            Die();
        }

    }
    [Server]
    private void Die()
    {
        UnitManager.Instance.UnregisterUnits(new NetworkIdentityReference(gameObject.GetComponent<NetworkIdentity>()), unitType);
        NetworkServer.Destroy(gameObject);
    }
    #endregion
    #region Client
    private void UpdateHealthBar(int oldHealth, int newHeatlh)
    {
        healthBar.value = (float)((float)newHeatlh / (float)baseHp);
    }
    #endregion


}

