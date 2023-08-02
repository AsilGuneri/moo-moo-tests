using UnityEngine;
using Mirror;

public class ShieldController : NetworkBehaviour
{
    public float shieldDistance = 5.0f;
    public float yOffset = 1.0f; // Y-axis offset to ensure the shield is above the ground

    private Transform protectedTarget;
    private Transform shieldedUnit;

    public void SetupShield(Transform protectedTarget, Transform shieldedUnit)
    {
        this.protectedTarget = protectedTarget;
        this.shieldedUnit = shieldedUnit;
        this.shieldedUnit.GetComponent<UnitController>().Health.OnDeath += DestroySelf;
        UpdateShieldPosition();
        NetworkServer.Spawn(gameObject, connectionToClient);

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
