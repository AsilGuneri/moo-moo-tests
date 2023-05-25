using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    public List<MyPool> pools = new List<MyPool>();

    private Dictionary<Guid, GameObject> assetIdToPrefabMap = new Dictionary<Guid, GameObject>();


    void Start()
    {
        InitializeAllPools();
        Instance = this;
        foreach (var pool in pools)
        {
            NetworkClient.RegisterPrefab(pool.Prefab, SpawnHandler, UnspawnHandler);
            assetIdToPrefabMap[pool.Prefab.GetComponent<NetworkIdentity>().assetId] = pool.Prefab;
        }
    }
    // used by NetworkClient.RegisterPrefab
    GameObject SpawnHandler(SpawnMessage msg)
    {
        if (assetIdToPrefabMap.TryGetValue(msg.assetId, out var prefab))
        {
            return Get(prefab, msg.position, msg.rotation);
        }
        else
        {
            Debug.LogError("No registered prefab found for assetId: " + msg.assetId);
            return null;
        }
    }

    // used by NetworkClient.RegisterPrefab
    void UnspawnHandler(GameObject spawned) => Return(spawned);

    void OnDestroy()
    {
        foreach (var pool in pools)
        {
            NetworkClient.UnregisterPrefab(pool.Prefab);
        }
    }

    void InitializeAllPools()
    {
        // create pool with generator function
        foreach(var myPool in pools)
        {
            myPool.pool = new Pool<GameObject>(() => CreateNew(myPool), myPool.Size);
        }
    }

    GameObject CreateNew(MyPool poolOfObj)
    {
        // use this object as parent so that objects dont crowd hierarchy
        GameObject next = Instantiate(poolOfObj.Prefab, transform);
        next.name = $"{poolOfObj.Prefab.name}_pooled_{poolOfObj.currentCount}";
        next.SetActive(false);
        poolOfObj.currentCount++;
        return next;
    }

    // Used to take Object from Pool.
    // Should be used on server to get the next Object
    // Used on client by NetworkClient to spawn objects
    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        MyPool poolOfObj = GetPool(prefab);
        GameObject next = poolOfObj.pool.Get();

        // set position/rotation and set active
        next.transform.position = position;
        next.transform.rotation = rotation;
        next.SetActive(true);
        return next;
    }

    // Used to put object back into pool so they can b
    // Should be used on server after unspawning an object
    // Used on client by NetworkClient to unspawn objects
    public void Return(GameObject spawned)
    {
        MyPool poolOfObj = GetPool(spawned);

        // disable object
        spawned.SetActive(false);

        // add back to pool
        poolOfObj.pool.Return(spawned);
    }

    private MyPool GetPool(GameObject prefab)
    {
        foreach (var pool in pools)
        {
            if (pool.Key == prefab.GetComponent<PoolObject>().Key)
                return pool;
        }
        Debug.LogError("Pool returned null");
        return null;
    }
}

[System.Serializable]
public class MyPool
{
    public GameObject Prefab;
    public int Size;
    public string Key;
    public Pool<GameObject> pool;
    public int currentCount;
}