using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MyBox;

public class FollowingCamera : MonoBehaviour
{
    public GameObject PlayerFollower;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float cornerThickness;
    [SerializeField] private float cornerMovementSpeed;
    [Separator("Mouse Wheel Zoom")]
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float scrollSpeed;


    private Transform _playerTransform;
    private bool _isLocked;
    private Vector3 _positionDif;
    private float _cameraDistance;

    public bool IsLocked = true ;
    public GameObject followEmptyObj;

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z)) ToggleLock();
        ZoomInOut();

        if (IsLocked) return;

        
        if (Input.GetKey(KeyCode.Space)) CenterCamera();

        if (Input.mousePosition.x >= Screen.width - cornerThickness) 
        {
            followEmptyObj.transform.position += new Vector3(Time.deltaTime * cornerMovementSpeed, 0, 0);
        }
        else if(Input.mousePosition.x <= cornerThickness)
        {
            followEmptyObj.transform.position -= new Vector3(Time.deltaTime * cornerMovementSpeed, 0, 0);
        }

        if (Input.mousePosition.y >= Screen.height - cornerThickness)
        {
            followEmptyObj.transform.position += new Vector3(0, 0, Time.deltaTime * cornerMovementSpeed);

        }
        else if (Input.mousePosition.y <= cornerThickness)
        {
            followEmptyObj.transform.position -= new Vector3(0, 0, Time.deltaTime * cornerMovementSpeed);
        }
    }
    public void SetupCinemachine(Transform playerTransform)
    {
        PlayerFollower.GetComponent<FollowPosition>().TargetTransform = playerTransform;
        _playerTransform = playerTransform;
        cinemachineVirtualCamera.m_Follow = PlayerFollower.transform;
        _positionDif = cinemachineVirtualCamera.transform.position - playerTransform.position;

    }
    private void ToggleLock()
    {
        IsLocked = !IsLocked;
        if (IsLocked)
        {
            cinemachineVirtualCamera.m_Follow = PlayerFollower.transform;
        }
        else
        {
            followEmptyObj.transform.position = PlayerFollower.transform.position;
            cinemachineVirtualCamera.m_Follow = followEmptyObj.transform;
            CenterCamera();
        }
    }
    private void CenterCamera()
    {
        followEmptyObj.transform.position = PlayerFollower.transform.position;
    }
    private void ZoomInOut()
    {
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance -= Input.mouseScrollDelta.y * Time.deltaTime * scrollSpeed;
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = Mathf.Clamp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance, minDistance, maxDistance);
    }
}
