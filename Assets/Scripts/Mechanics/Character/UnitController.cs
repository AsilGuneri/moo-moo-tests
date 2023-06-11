using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UnitController : NetworkBehaviour
{
    //others


    //temp variables
    public TargetController TargetController { get => targetController; }
    public float attackRange;
    public float attackSpeed;
    public float speed;
    public UnitType unitType;
    public List<UnitType> enemyList;

    protected TargetController targetController;
    protected BasicAttackController attackController;
    [Range(0, 1f)] public float animAttackPoint;


    protected virtual void Awake()
    {
        attackController = GetComponent<BasicAttackController>();
        targetController = GetComponent<TargetController>();
    }

    // Update is called once per frame
   
    
   
    protected bool IsEnemy(RaycastHit hitInfo)
    {
        if (hitInfo.transform.TryGetComponent(out UnitController unit))
        {
            return enemyList.Contains(unit.unitType);
        }
        Debug.Log("Unit doesn't have UnitController component");
        return false;
    }
}
