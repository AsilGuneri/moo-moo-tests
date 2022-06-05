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
        }

    }
    #endregion
    #region Client
    private void UpdateHealthBar(int oldHealth, int newHeatlh)
    {
        healthBar.value = (float)((float)newHeatlh / (float)_maxHealth);
    }
    #endregion

}
/* protected int CurrentHp 
 {
     get
     {
         float percentage = ((float) _currentHealth / (float)_maxHealth);
         healthBar.value = percentage;
         return _currentHealth; 
     }
     set
     {
         _currentHealth = value;
         float percentage = ((float)_currentHealth / (float)_maxHealth);
         healthBar.value = percentage;
     }
 }*/

//private void Start()
//{
//    if (TeamId == 1) UnitManager.Instance.RegisterAllyUnits(this);
//    CurrentHp = _maxHealth;
//}
//protected virtual void RegenerateHp()
//{

//}
//public virtual void TakeDamage(int damage)
//{
//    CurrentHp -= damage;
//  //  if (_currentHp <= 0) Die();
//}
//protected virtual void Heal(int amount)
//{
//    CurrentHp += amount;
//}
//public virtual void Die()
//{
//    gameObject.SetActive(false);
//    Respawn();
//}
//protected virtual void Respawn()
//{
//    transform.position = _respawnPos;
//    gameObject.SetActive(true);
//    CurrentHp = _maxHealth;
//}

//   private void 

public enum UnitType
{
    Unit,
    Building
}
