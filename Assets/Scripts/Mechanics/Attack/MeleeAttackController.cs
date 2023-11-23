using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackController : BasicAttackController
{
    protected override void OnEachAttackEnd()
    {

    }

    protected override void OnAttackImpact()
    {
        base.OnAttackImpact();
        DealDamageToCurrentTarget();
    }

    protected override void OnAttackStart()
    {

    }
    [Command(requiresAuthority = false)]
    private void DealDamageToCurrentTarget()
    {
        controller.TargetController.Target.GetComponent<Health>().TakeDamage(GetActualDamage(), transform);
        OnHit();
    }
}
