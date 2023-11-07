using DG.Tweening;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TowerController : UnitController
{

    [SerializeField] private GameObject visualEffectObject;

    protected bool isActive = false;
    private float counter = 0;

    private Material visualEffectMaterial;

    protected override void Awake() // I use Awake so it's initialized early.
    {
        base.Awake();
        if (visualEffectObject)
            visualEffectMaterial = visualEffectObject.GetComponent<Renderer>().material;
    }
    void Start()
    {
        StartUnit();
    }

    public void StartTower()
    {
        isActive = true;
        StartCoroutine(TowerActivision());
        attackController.OnEachAttackStart += OnEachAttackStart;
        attackController.OnActualAttackMoment += OnProjectileSpawn;
        attackController.OnEndAttack += OnAttackEnd;
    }
    public void StopTower()
    {
        StopCoroutine(TowerActivision());
        isActive = false;
        attackController.OnEachAttackStart -= OnEachAttackStart;
        attackController.OnActualAttackMoment -= OnProjectileSpawn;
        attackController.OnEndAttack -= OnAttackEnd;
    }
    public override void RpcOnRegister()
    {
        StartTower();
    }
    public override void OnDeath(Transform killer)
    {
        base.OnDeath(killer);
        StopTower();
    }

    private void OnEachAttackStart()
    {
        AnimateTowerHit();
    }

    private void OnProjectileSpawn()
    {

    }
    private void OnAttackEnd()
    {

    }
    private IEnumerator TowerActivision()
    {
        counter = 0;
        float tickInterval = 0.1f;
        while (isActive)
        {
            yield return Extensions.GetWait(tickInterval);
            counter += tickInterval;

            if (targetController.HasTarget() && !attackController.IsAttacking)
            {
                attackController.StartAutoAttack();
            }
            else
            {
                var target = UnitManager.Instance.GetClosestEnemy(transform.position, this);
                if (!target) continue;
                if (target != targetController.Target)
                    targetController.SetTarget(target.GetComponent<NetworkIdentity>());
            }
        }
    }


    private void AnimateTowerHit()
    {
        Extensions.GetAttackTimes(attackSpeed, attackController.AnimAttackPoint, out float timeBeforeAttack, out float timeAfterAttack);
        float fadeOutTime = 0.1f;

        if (visualEffectMaterial)
        {
            // Kill any existing DOTweens to prevent overlaps or conflicts.
            visualEffectMaterial.DOKill();

            // Tween Final Power from its current value to 10 over timeBeforeAttack seconds
            DOTween.To(() => visualEffectMaterial.GetFloat("_FinalPower"),
            x => visualEffectMaterial.SetFloat("_FinalPower", x),
            12, timeBeforeAttack).
            OnComplete(() =>
            {
                DOTween.To(() => visualEffectMaterial.GetFloat("_FinalPower"),
                            x => visualEffectMaterial.SetFloat("_FinalPower", x),
                            2, fadeOutTime).SetEase(Ease.OutCubic)
                            .OnComplete(() =>
                              {
                                  DOTween.To(() => visualEffectMaterial.GetFloat("_FinalPower"),
                                              x => visualEffectMaterial.SetFloat("_FinalPower", x),
                                              7, timeAfterAttack - fadeOutTime).SetEase(Ease.Linear);
                              });
            });
        }
    }


}
