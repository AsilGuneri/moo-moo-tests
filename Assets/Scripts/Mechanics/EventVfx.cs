using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventVfx : MonoBehaviour
{
    /// <summary>
    /// Vfx spawns when ; For range : projectile spawn moment, for melee : hit moment.
    /// </summary>
    [SerializeField] private GameObject onActualAttackFxPrefab;
    [SerializeField] private Transform actualAttackFxPosRef;
    [SerializeField] private Vector3 actualAttackFxRotation;


    private UnitController controller;

    private void Awake()
    {
        controller = GetComponent<UnitController>();
    }
    private void Start()
    {
        if (onActualAttackFxPrefab) controller.AttackController.OnActualAttackMoment += OnActualAttackStart;

    }
    /// <summary>
    /// For range : projectile spawn moment, for melee : hit moment.
    /// </summary>
    private void OnActualAttackStart()
    {
        ObjectPooler.Instance.CmdSpawnFromPool(onActualAttackFxPrefab.name,
            actualAttackFxPosRef.position, Quaternion.Euler(actualAttackFxRotation));
    }
}
