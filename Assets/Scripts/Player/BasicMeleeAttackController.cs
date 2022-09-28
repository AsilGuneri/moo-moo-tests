using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class BasicMeleeAttackController : NetworkBehaviour
{
    private BasicAnimationController pac;
    private TargetController tc;
    private UnitMovementController umc;

    private bool isAttacking;
    private float attackSpeed;
    private float counter;
    private int damage;
    private float range;

    private void Awake()
    {
        pac = GetComponent<BasicAnimationController>();
        tc = GetComponent<TargetController>();
        umc = GetComponent<UnitMovementController>();
    }
    [ClientCallback]
    private void Update()
    {
        if (!hasAuthority) return;
        if (counter <= (1 / attackSpeed)) counter += Time.deltaTime;
        if (tc.Target == null)
        {
            StopAttacking();
            ResetStoppingDistance();
            StopChasing();
            return;
        }
        if (Vector2.Distance(Extensions.Vector3ToVector2(tc.Target.transform.position), Extensions.Vector3ToVector2(transform.position)) > range && !isAttacking)
        {
            ChaseToAttack();
        }
        else if (counter >= (1 / attackSpeed) && !isAttacking)
        {
            Attack();
        }
    }
    private void StopAttacking()
    {

    }
    private void ResetStoppingDistance()
    {

    }
    private void StopChasing()
    {

    }
    private void ChaseToAttack()
    {

    }
    private void Attack()
    {
        transform.LookAt(new Vector3(tc.Target.transform.position.x, transform.position.y, tc.Target.transform.position.z));
        if (umc) umc.ClientStop();
        if (pac) pac.OnAttackStart(attackSpeed);
        isAttacking = true;
        StartCoroutine(nameof(DelayMeleeAttack));
        counter = 0;
    }
    private IEnumerator DelayMeleeAttack()
    {
        yield return new WaitForSeconds((1 / attackSpeed) / 2);
        DealDamage();
        yield return new WaitForSeconds((1 / attackSpeed) / 2);
    }
    private void DealDamage()
    {
        tc.Target.GetComponent<Health>().TakeDamage(damage);
    }
}
