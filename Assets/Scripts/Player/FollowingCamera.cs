using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowingCamera : MonoBehaviour
{
    public GameObject PlayerFollower;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float cornerThickness;
    [SerializeField] private float cornerMovementSpeed;

    private CinemachineFramingTransposer _framingTransposer;
    private bool _isLocked;

    public bool IsLocked { get { return _isLocked; } private set { _isLocked = value; } }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z)) ToggleLock();
        if (!IsLocked) return;

        
        if (Input.GetKeyDown(KeyCode.Space)) CenterCamera();

        if (Input.mousePosition.x >= Screen.width - cornerThickness) 
        {
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.x += Time.deltaTime * cornerMovementSpeed;
        }
        else if(Input.mousePosition.x <= cornerThickness)
        {
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.x -= Time.deltaTime * cornerMovementSpeed;
        }

        if (Input.mousePosition.y >= Screen.height - cornerThickness)
        {
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.z += Time.deltaTime * cornerMovementSpeed;

        }
        else if (Input.mousePosition.y <= cornerThickness)
        {
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.z -= Time.deltaTime * cornerMovementSpeed;
        }
    }
    public void SetupCinemachine(Transform playerTransform)
    {
        PlayerFollower.GetComponent<FollowPosition>().TargetTransform = playerTransform;
        cinemachineVirtualCamera.m_Follow = PlayerFollower.transform;
    }
    private void ToggleLock()
    {
        IsLocked = !IsLocked;
        if (IsLocked) CenterCamera();
    }
    private void CenterCamera()
    {
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = Vector3.zero;
    }
}
