using Mirror;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolObject : NetworkBehaviour
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
        BackToPool();
    }

    public void BackToPool()
    {
        if (gameObject.TryGetComponent(out NetworkIdentity netId))
        {
            PrefabPoolManager.Instance.PutBackInPool(gameObject);
        }
        else
        {
            PrefabPoolManager.Instance.PutBackInPool(gameObject);

        }
    }

    protected virtual void OnReturn()
    {
        if(TryGetComponent(out BasicAttackController controller))
        {
            controller.ResetAttackController();
        }
       
    }
    protected virtual void OnSpawn()
    {
        
    }

    public override void OnStartServer()
    {
        OnSpawn();
    }
    public override void OnStopServer()
    {
        OnReturn();
    }
}

