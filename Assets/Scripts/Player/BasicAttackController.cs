using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Mirror;
using UnityEngine.UIElements;
using MyBox;

public class BasicAttackController : NetworkBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Separator("Script References")]
    [SerializeField] private TargetController tc;
    [SerializeField] private UnitMovementController umc;
    [SerializeField] private PlayerAnimationController pac;


    private float counter;
    private float additionalAttackSpeed;
    private bool isAttacking;
    
    public bool IsAttacking
    {
        get => isAttacking;
        set => isAttacking = value;
    }
    public float AttackSpeed
    {
        get => attackSpeed;
    }
    public float AdditionalAttackSpeed
    {
        get => additionalAttackSpeed;
        set => additionalAttackSpeed = value;
    }

    #region Server
    [Server]
    private bool CanFireAtTarget()
    {
        return (tc.Target.transform.position - transform.position).sqrMagnitude > attackRange * attackRange;
    }
    [Command]
    private void CmdSpawnProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetupProjectile(tc.Target, damage);
        NetworkServer.Spawn(projectile, connectionToClient);
    }
    #endregion
    #region Client
    [ClientCallback]
    private void Update()
    {
        if (!hasAuthority) return;
        if (counter <= (1 / AttackSpeed)) counter += Time.deltaTime;
        if (!tc.Target) 
        {
            if (isAttacking)
            {
                isAttacking = false;
                if (pac) pac.OnAttackEnd();
            }
            agent.stoppingDistance = 0; 
            return; 
        }
        if (Vector2.Distance(Extensions.Vector3ToVector2(tc.Target.transform.position), Extensions.Vector3ToVector2(transform.position)) > range)
        {
            umc.ClientMove(tc.Target.transform.position, true, range);
        }
        else if (counter >= (1 / AttackSpeed))
        {
            transform.LookAt(new Vector3(tc.Target.transform.position.x, transform.position.y, tc.Target.transform.position.z));
            if(umc) umc.ClientStop();
            if(pac) pac.OnAttackStart(attackSpeed);
            isAttacking = true;
            CmdSpawnProjectile();
            counter = 0;
        }       
    }
    #endregion
}
