using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIndicator : MonoBehaviour
{
    bool isActive = false;
    private Camera mainCamera;
    private int groundLayerMask;

    private void Start()
    {
        mainCamera = Camera.main; // Assuming you have one main camera in the scene.
        groundLayerMask = 1 << 6; // The layer mask for the ground (since you mentioned ground layer is 6).
    }

    public void Setup()
    {
        isActive = true;
    }

    public void EndIndicator()
    {
        isActive = false;
        PrefabPoolManager.Instance.PutBackInPool(gameObject);
    }

    private void Update()
    {
        if (isActive)
        {
            FollowMouse();
        }
    }

    private void FollowMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
        {
            transform.position = hit.point; // Move the SkillIndicator to the hit point on the ground.
        }
    }
}
