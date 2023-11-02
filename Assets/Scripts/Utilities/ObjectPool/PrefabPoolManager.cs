using System.Collections.Generic;
using Mirror;
using UnityEngine;


[System.Serializable]
public class PoolConfig
{
    public int startSize = 5;
    public int maxSize = 20;
    public PoolObject prefab;
    public Queue<GameObject> pool;
    public int currentCount;
}

public class PrefabPoolManager : NetworkBehaviour
{
    private Dictionary<uint, PoolConfig> assetIdToConfigMap = new Dictionary<uint, PoolConfig>();

    public static PrefabPoolManager Instance;

    [Header("Settings")]
    public List<PoolConfig> poolConfigs = new List<PoolConfig>();

    void Start()
    {
        Instance = this;
        foreach (var config in poolConfigs)
        {
            InitializePool(config);
            NetworkClient.RegisterPrefab(config.prefab.gameObject, SpawnHandler, UnspawnHandler);
            assetIdToConfigMap[config.prefab.GetComponent<NetworkIdentity>().assetId] = config;
        }
    }

    void OnDestroy()
    {
        foreach (var config in poolConfigs)
        {
            NetworkClient.UnregisterPrefab(config.prefab.gameObject);
        }
    }

    private void InitializePool(PoolConfig config)
    {
        config.pool = new Queue<GameObject>();
        for (int i = 0; i < config.startSize; i++)
        {
            GameObject next = CreateNew(config);

            config.pool.Enqueue(next);
        }
    }

    GameObject CreateNew(PoolConfig config)
    {
        if (config.currentCount > config.maxSize)
        {
            Debug.LogError($"Pool for {config.prefab.name} has reached max size of {config.maxSize}");
            return null;
        }
        if(config.prefab == null)
        {
            Debug.Log("asilxx " + config.prefab.name);
        }
        GameObject next = Instantiate(config.prefab.gameObject, transform);
        next.name = $"{config.prefab.name}_pooled_{config.currentCount}";
        next.SetActive(false);
        config.currentCount++;
        return next;
    }

    GameObject SpawnHandler(SpawnMessage msg)
    {
        return GetFromPool(msg.assetId, msg.position, msg.rotation);
    }

    void UnspawnHandler(GameObject spawned)
    {
        PutBackInPool(spawned);
    }

    public GameObject GetFromPool(uint assetId, Vector3 position, Quaternion rotation)
    {
        if (assetIdToConfigMap.TryGetValue(assetId, out PoolConfig config))
        {
            GameObject next = config.pool.Count > 0
                ? config.pool.Dequeue()
                : CreateNew(config);

            if (next == null) { return null; }

            next.transform.position = position;
            next.transform.rotation = rotation;
            next.SetActive(true);
            return next;
        }
        else
        {
            Debug.LogError($"No pool configuration found for asset ID: {assetId}");
            return null;
        }
    }
    public GameObject GetFromPool(GameObject netIdObject, Vector3 position, Quaternion rotation)
    {
        if (netIdObject.TryGetComponent(out NetworkIdentity netId))
        {
            return GetFromPool(netId.assetId, position, rotation);
        }
        else
        {
            Debug.LogError("Could not find networkId therefore no assetId");
            return null;
        }
    }


    public void PutBackInPool(GameObject spawned)
    {
        var assetId = spawned.GetComponent<NetworkIdentity>().assetId;
        if (assetIdToConfigMap.TryGetValue(assetId, out PoolConfig config))
        {
            spawned.SetActive(false);
            config.pool.Enqueue(spawned);
        }
        else
        {
            Debug.LogError($"No pool configuration found for asset ID: {assetId}");
        }
    }

}

