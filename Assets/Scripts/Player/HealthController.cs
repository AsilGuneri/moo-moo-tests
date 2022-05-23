using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public UnitType unitType;
    public int TeamId;

    [SerializeField] private Slider healthBar;
    [SerializeField] private int _maxHp;
    [SerializeField] protected Vector3 _respawnPos;
    protected int _currentHp;

    protected int CurrentHp 
    {
        get
        {
            float percentage = ((float) _currentHp / (float)_maxHp);
            healthBar.value = percentage;
            return _currentHp; 
        }
        set
        {
            _currentHp = value;
            float percentage = ((float)_currentHp / (float)_maxHp);
            healthBar.value = percentage;
        }
    }

    private void Start()
    {
        if (TeamId == 1) UnitManager.Instance.RegisterAllyUnits(this);
        CurrentHp = _maxHp;
    }
    protected virtual void RegenerateHp()
    {

    }
    public virtual void TakeDamage(int damage)
    {
        CurrentHp -= damage;
      //  if (_currentHp <= 0) Die();
    }
    protected virtual void Heal(int amount)
    {
        CurrentHp += amount;
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
        CurrentHp = _maxHp;
    }

 //   private void 
}
public enum UnitType
{
    Unit,
    Building
}
