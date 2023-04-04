using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class BasicMeleeAttackController : ABasicAttackController
{
    private IEnumerator DelayMeleeAttack()
    {
        yield return new WaitForSeconds((1 / baseStats.AttackSpeed) / 2);
        DealDamage();
        yield return new WaitForSeconds((1 / baseStats.AttackSpeed) / 2);
    }
    private void DealDamage()
    {
        tc.Target.GetComponent<Health>().TakeDamage(baseStats.Damage, transform);
    }

    protected override void StopAttacking()
    {   
        if (IsAttacking)
        {
            IsAttacking = false;
            StopCoroutine(nameof(DelayMeleeAttack));
        }
    }

    protected override void StartAttacking()
    {
        transform.LookAt(new Vector3(tc.Target.transform.position.x, transform.position.y, tc.Target.transform.position.z));
        if (umc) umc.ClientStop();
        if (ac) ac.OnAttackStart(baseStats.AttackSpeed);
        IsAttacking = true;
        StartCoroutine(nameof(DelayMeleeAttack));
        counter = 0;
    }
}
