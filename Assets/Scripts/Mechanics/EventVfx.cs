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


    private PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }
    private void Start()
    {
        controller.AttackController.OnActualAttackMoment += OnActualAttackStart;
    }
    /// <summary>
    /// For range : projectile spawn moment, for melee : hit moment.
    /// </summary>
    private void OnActualAttackStart()
    {
        ObjectPooler.Instance.CmdSpawnFromPool(onActualAttackFxPrefab.name,
            actualAttackFxPosRef.position, Quaternion.identity);
    }
}
