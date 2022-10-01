using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SelectIndicator : MonoBehaviour
{
    [SerializeField] private Transform staticIndicator;
    [SerializeField] private Transform dynamicIndicator;


    private bool _isFollowing = false;
    private Transform _target;

    public Transform StaticIndicator
    {
        get => staticIndicator;
    }
    public Transform DynamicIndicator
    {
        get => dynamicIndicator;
        set => dynamicIndicator = value;
    }
   
    private void LateUpdate()
    {
       // if (!hasAuthority) return;
        if (_isFollowing)
        {
            DynamicIndicator.position = _target.position;
        }
    }
    public void SetupIndicator(Transform target, bool isFollowing)
    {
        _isFollowing = isFollowing;
        DynamicIndicator.gameObject.SetActive(isFollowing);
        _target = target;
        if (!_target) return;
        DynamicIndicator.localScale = target.localScale;
        DynamicIndicator.localRotation = target.localRotation;
    }
}
