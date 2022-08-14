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

    private Vector3 _initialCamPos;
    private bool _isLocked;

    public bool IsLocked { get { return _isLocked; } private set { _isLocked = value; } }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z)) ToggleLock();
        if (IsLocked) return;

        
        if (Input.GetKeyDown(KeyCode.Space)) CenterCamera();

        if (Input.mousePosition.x >= Screen.width - cornerThickness) 
        {
            /*cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.x*/
            cinemachineVirtualCamera.transform.position += new Vector3(Time.deltaTime * cornerMovementSpeed, 0, 0);
        }
        else if(Input.mousePosition.x <= cornerThickness)
        {
            /*cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.x */
            cinemachineVirtualCamera.transform.position -= new Vector3(Time.deltaTime * cornerMovementSpeed, 0, 0);
        }

        if (Input.mousePosition.y >= Screen.height - cornerThickness)
        {
            /*cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.z*/
            cinemachineVirtualCamera.transform.position += new Vector3(0, 0, Time.deltaTime * cornerMovementSpeed);

        }
        else if (Input.mousePosition.y <= cornerThickness)
        {
            /*cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.z*/
            cinemachineVirtualCamera.transform.position -= new Vector3(0, 0, Time.deltaTime * cornerMovementSpeed);
        }
    }
    public void SetupCinemachine(Transform playerTransform)
    {
        PlayerFollower.GetComponent<FollowPosition>().TargetTransform = playerTransform;
        cinemachineVirtualCamera.m_Follow = PlayerFollower.transform;
        _initialCamPos = cinemachineVirtualCamera.transform.position;

    }
    private void ToggleLock()
    {
        IsLocked = !IsLocked;
        if (IsLocked)
        {
            cinemachineVirtualCamera.m_Follow = PlayerFollower.transform;
            CenterCamera();
        }
        else
        {
            CenterCamera();
            cinemachineVirtualCamera.m_Follow = null;
        }
    }
    private void CenterCamera()
    {
        // cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = Vector3.zero;
        cinemachineVirtualCamera.transform.position = _initialCamPos;
    }
}
