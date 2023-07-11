using Mirror;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolObject : MonoBehaviour
{
    private string prefabName;
    [SerializeField] bool useLifeTime;
    [ConditionalField(nameof(useLifeTime))][SerializeField] private float lifeTime;
    public string PrefabName
    {
        get
        {
            return prefabName;
        }
    }
    private void Awake()
    {
        prefabName = name;
    }
    private void OnEnable()
    {
        if (useLifeTime && lifeTime > 0) 
            StartCoroutine(ReturnToPool());

    }
    private void OnDisable()
    {
        if (useLifeTime && lifeTime > 0)
            StopCoroutine(ReturnToPool());
    }
    private IEnumerator ReturnToPool()
    {
        yield return Extensions.GetWait(lifeTime);
        if (gameObject.TryGetComponent(out NetworkIdentity netId))
            ObjectPooler.Instance.CmdReturnToPool(gameObject.GetComponent<NetworkIdentity>().netId);
        else
            ObjectPooler.Instance.ReturnToPool(gameObject);
    }
 
}

