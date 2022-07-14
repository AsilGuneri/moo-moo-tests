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

    [SerializeField] private int _maxHealth;
    [SerializeField] private Slider healthBar;
    [SerializeField] private UnitType unitType;

    [SyncVar(hook = nameof(UpdateHealthBar))] protected int _currentHealth;

    public event Action ServerOnDeath;

    #region Server
    public override void OnStartServer()
    {
        _currentHealth = _maxHealth;

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
        UnitManager.Instance.UnregisterUnits(gameObject, unitType);
        Destroy(gameObject);
    }
    #endregion
    #region Client
    private void UpdateHealthBar(int oldHealth, int newHeatlh)
    {
        healthBar.value = (float)((float)newHeatlh / (float)_maxHealth);
    }
    #endregion


}

