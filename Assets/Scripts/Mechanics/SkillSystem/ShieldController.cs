﻿using UnityEngine;
using Mirror;

public class ShieldController : NetworkBehaviour
{
    [SerializeField] private int shieldMaxHitPoint;
    [SerializeField] private float shieldDistance = 5.0f;
    [SerializeField] private float yOffset = 1.0f; // Y-axis offset to ensure the shield is above the ground
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private float minDissolve = 0.0f;
    [SerializeField] private float maxDissolve = 1.0f;

    private Transform protectedTarget;
    private Transform shieldedUnit;
    private int hitCounter;
    private Renderer shieldRenderer;
    private UnitController unitController;

    public void SetupShield(Transform protectedTarget, Transform shieldedUnit)
    {
        unitController = shieldedUnit.GetComponent<UnitController>();
        shieldRenderer = shieldObject.GetComponent<Renderer>();
        this.protectedTarget = protectedTarget;
        this.shieldedUnit = shieldedUnit;
        unitController.Health.OnDeath += DestroySelf;
        UpdateShieldPosition();
        hitCounter = 0;
        NetworkServer.Spawn(gameObject, connectionToClient);
    }

    private void OnTriggerStay(Collider other)
    {
        Projectile projectile = other.GetComponent<Projectile>();

        if (projectile != null && projectile.BelongsToEnemy(unitController.unitType)) // Corrected line
        {
            TakeShieldDamage();
            projectile.DestroySelf();
        }
    }

    private void TakeShieldDamage()
    {
        hitCounter++;
        float dissolveValue = minDissolve + ((maxDissolve - minDissolve) / shieldMaxHitPoint * hitCounter);
        shieldRenderer.material.SetFloat("_Dissolve", dissolveValue);
        if (hitCounter >= shieldMaxHitPoint)
        {
            DestroySelf();
        }
    }
    private void DestroySelf()
    {
        shieldedUnit.GetComponent<UnitController>().Health.OnDeath -= DestroySelf;
        GetComponent<PoolObject>().BackToPool();
    }

    private void UpdateShieldPosition()
    {
        Vector3 directionToShieldedUnit = (protectedTarget.position - shieldedUnit.position).normalized;
        Vector3 shieldPosition = shieldedUnit.position + directionToShieldedUnit * shieldDistance;
        shieldPosition.y += yOffset; // Add the Y-axis offset
        transform.position = shieldPosition;

        // Calculate the direction from the shieldedUnit to the shield's position
        Vector3 directionFromCenter = (transform.position - shieldedUnit.position).normalized;

        // Get the rotation that looks in that direction
        Quaternion lookRotation = Quaternion.LookRotation(directionFromCenter);

        // Extract the Y-axis rotation
        float yRotation = lookRotation.eulerAngles.y;

        // Apply only the Y-axis rotation to the current rotation
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yRotation, transform.rotation.eulerAngles.z);
    }

    void Update()
    {
        if (protectedTarget != null && shieldedUnit != null)
        {
            UpdateShieldPosition();
        }
    }
}
