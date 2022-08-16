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
    [SerializeField] private float attackSpeed;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    private TargetController tc;
    private UnitMovementController umc;
    private PlayerAnimationController pac;
    [SerializeField] private NavMeshAgent agent;



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
    public float Range
    {
        get => range;
    }
    private void Awake()
    {
        tc = GetComponent<TargetController>();
        umc = GetComponent<UnitMovementController>();
        pac = GetComponent<PlayerAnimationController>();    
      //  agent = GetComponent<NavMeshAgent>();
    }

    #region Server
    [Command]
    private void CmdSpawnProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetupProjectile(tc.Target, damage);
        NetworkServer.Spawn(projectile, connectionToClient);
        Debug.Log("spawned projectile");
    }
    private IEnumerator DelayProjectileSpawn()
    {
        yield return new WaitForSeconds((1 / attackSpeed) / 2);
        CmdSpawnProjectile();
        yield return new WaitForSeconds(3f);
        isAttacking = false;

    }
    #endregion
    #region Client
    [ClientCallback]
    private void Update()
    {
        if (!hasAuthority) return;
        if (counter <= (1 / AttackSpeed)) counter += Time.deltaTime;
        if (tc.Target == null) 
        {
            if (isAttacking)
            {
                isAttacking = false;
                StopCoroutine(nameof(DelayProjectileSpawn));
                if (pac) pac.OnAttackEnd();

            }
            agent.stoppingDistance = 0; 
            return; 
        }
        if (Vector2.Distance(Extensions.Vector3ToVector2(tc.Target.transform.position), Extensions.Vector3ToVector2(transform.position)) > range && !isAttacking)
        {
            umc.ClientMove(tc.Target.transform.position, true, range);
        }
        else if (counter >= (1 / AttackSpeed))
        {
            transform.LookAt(new Vector3(tc.Target.transform.position.x, transform.position.y, tc.Target.transform.position.z));
            if(umc) umc.ClientStop();
            if(pac) pac.OnAttackStart(attackSpeed);
            isAttacking = true;
            StartCoroutine(nameof(DelayProjectileSpawn));
            counter = 0;
        }       
    }
    #endregion
}
