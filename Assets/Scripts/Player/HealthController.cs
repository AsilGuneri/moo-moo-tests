using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int _maxHp;
    [SerializeField] protected Vector3 _respawnPos;
    protected int _currentHp;
    

    protected virtual void RegenerateHp()
    {

    }
    public virtual void TakeDamage(int damage)
    {
        _currentHp -= damage;
      //  if (_currentHp <= 0) Die();
    }
    protected virtual void Heal(int amount)
    {
        _currentHp += amount;
    }
    public virtual void Die()
    {
        gameObject.SetActive(false);
        Respawn();
    }
    protected virtual void Respawn()
    {
        transform.position = _respawnPos;
        gameObject.SetActive(true);
        _currentHp = _maxHp;
    }

 //   private void 
}
