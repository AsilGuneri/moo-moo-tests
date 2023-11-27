using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectController : NetworkBehaviour
{
    UnitController controller;
    Coroutine currentFireEffectCoroutine;
    Coroutine currentIceEffectCoroutine;


    private void Awake()
    {
        controller = GetComponent<UnitController>();
    }

    [Server]
    public void ApplyStun(float time)
    {
        if (controller.Health.IsDead) return;
        StartCoroutine(StunRoutine(time));
    }
    [Server]
    public void ApplyDamagePerSecond(float time, int dps, Transform dmgDealer)
    {
        if (controller.Health.IsDead) return;
        if (currentFireEffectCoroutine != null)
        {
            StopCoroutine(currentFireEffectCoroutine);
        }
        currentFireEffectCoroutine = StartCoroutine(BurnRoutine(time, dps, dmgDealer));
    }
    [Server]
    public void ApplySlow(float time, float ratio)
    {
        if (controller.Health.IsDead) return;
        if (currentFireEffectCoroutine != null) StopCoroutine(currentFireEffectCoroutine);
        currentIceEffectCoroutine = StartCoroutine(SlowRoutine(time, ratio));
    }

    IEnumerator StunRoutine(float time)
    {
        controller.Movement.BlockMovement();
        controller.AttackController.BlockAttacking();
        yield return Extensions.GetWait(time);
        controller.Movement.RemoveMovementBlock();
        controller.AttackController.RemoveAttackingBlock();
    }

    IEnumerator BurnRoutine(float time, int dps, Transform dmgDealer)
    {
        float endTime = Time.time + time;
        while (Time.time < endTime)
        {
            yield return Extensions.GetWait(1);
            controller.Health.TakeDamage(dps, dmgDealer);
        }
    }
    IEnumerator SlowRoutine(float time, float ratio)
    {
        controller.StatController.ChangeMoveSpeed(-ratio);
        yield return Extensions.GetWait(time);
        controller.StatController.ChangeMoveSpeed(ratio);
    }
}
