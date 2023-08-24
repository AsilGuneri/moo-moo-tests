using DG.Tweening;
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

            if (targetController.Target && !attackController.IsAttacking)
            {
                attackController.StartAutoAttack();
            }
            else
            {
                GameObject target = UnitManager.Instance.GetClosestEnemy(transform.position, this);
                if (target != targetController.Target)
                    targetController.SetTarget(target);
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
    private void UpdateVisualEffect(float currentCooldown, float totalCooldown)
    {
        if (visualEffectMaterial)
        {
            float normalizedValue = Mathf.Clamp01(currentCooldown / totalCooldown); // Ensures the value is between 0 and 1
            float finalPower = Mathf.Lerp(2, 10, normalizedValue); // Map this value between 2 and 10 for Final Power

            visualEffectMaterial.SetFloat("_FinalPower", finalPower); // Assuming the shader's internal name for the property is "_FinalPower"
        }
    }

}
