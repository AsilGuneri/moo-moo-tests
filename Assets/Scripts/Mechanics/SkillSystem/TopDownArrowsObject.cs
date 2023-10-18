using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TopDownArrowsObject : NetworkBehaviour
{
    [SerializeField] float expandTime = 1.0f;

    [SerializeField] CapsuleCollider dmgCollider;

    private Transform casterTransform;
    private List<GameObject> damageDealtEnemies = new List<GameObject>();

    public void Setup(Transform caster)
    {
        casterTransform = caster;
        ExpandCollider();
    }
    private void ExpandCollider()
    {
        var colliderTransform = dmgCollider.transform;
        colliderTransform.DOKill();
        colliderTransform.localScale = Vector3.zero;
        colliderTransform.DOScale(Vector3.one, expandTime).SetEase(Ease.Linear);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 9) return;
        if (damageDealtEnemies.Contains(other.gameObject)) return;
        var enemyUnit = other.gameObject.GetComponent<UnitController>();
        enemyUnit.Health.TakeDamage(25, casterTransform);
        damageDealtEnemies.Add(other.gameObject);

    }

}
