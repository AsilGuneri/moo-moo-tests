using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MyBox;
using UnityEngine.InputSystem;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] InputActionReference holdCenterActionRef;
    [Separator("Corner")]
    [SerializeField] private float cornerThickness;
    [SerializeField] private float cornerMovementSpeed;
    [Separator("Mouse Wheel Zoom")]
    [SerializeField] private Transform zoomTransform;
    [SerializeField] private float zoomSmoothness = 10.0f;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float scrollSpeed;

    Transform target;
    Vector3 initialOffset;
    bool isActive;
    bool isFollowing = true;
    bool isZooming;
    float targetDistance;
    bool holdingCenterKey = false;

    private void Start()
    {
        holdCenterActionRef.action.performed += x => { holdingCenterKey = true; };
        holdCenterActionRef.action.canceled += x => { holdingCenterKey = false; };
    }

    private void Update()
    {
        if (!isActive) return;

        if (Input.GetKeyDown(KeyCode.Y)) ToggleLock();

        if (Input.mouseScrollDelta.y != 0)
        {
            targetDistance = zoomTransform.localPosition.z + (Input.mouseScrollDelta.y * scrollSpeed);
            targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
        }
        isZooming = Mathf.Abs(zoomTransform.localPosition.z - targetDistance) > 0.01f || Input.mouseScrollDelta.y != 0;
    }
    void LateUpdate()
    {
        if (!isActive) return;
        if (isZooming)
        {
            ZoomInOut();
        }
        if (isFollowing || holdingCenterKey)
        {
            FollowTarget();
            return;
        }


        if (Input.mousePosition.x >= Screen.width - cornerThickness)
        {
            transform.position += new Vector3(Time.deltaTime * cornerMovementSpeed, 0, 0);
        }
        else if (Input.mousePosition.x <= cornerThickness)
        {
            transform.position -= new Vector3(Time.deltaTime * cornerMovementSpeed, 0, 0);
        }

        if (Input.mousePosition.y >= Screen.height - cornerThickness)
        {
            transform.position += new Vector3(0, 0, Time.deltaTime * cornerMovementSpeed);

        }
        else if (Input.mousePosition.y <= cornerThickness)
        {
            transform.position -= new Vector3(0, 0, Time.deltaTime * cornerMovementSpeed);
        }
    }
    public void Setup(Transform playerTransform)
    {
        target = playerTransform;
        initialOffset = transform.position - target.position;
        isActive = true;
    }
    private void FollowTarget()
    {
        transform.position = target.position + initialOffset;
    }
    
    private void ToggleLock()
    {
        isFollowing = !isFollowing;

    }
    private void ZoomInOut()
    {
        Vector3 currentPos = zoomTransform.localPosition;
        Vector3 targetPos = new Vector3(0, 0, targetDistance);

        zoomTransform.localPosition = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * zoomSmoothness);
    }

}
