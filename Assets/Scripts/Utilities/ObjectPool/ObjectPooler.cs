using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class ObjectPooler : NetworkBehaviour
{
    public static ObjectPooler Instance;

    [SerializeField] private List<Pool> pools;

    private Dictionary<GameObject, Pool> poolDictionary;

    private void Start()
    {

        Instance = this;

        poolDictionary = new Dictionary<GameObject, Pool>();

        foreach (Pool pool in pools)
        {
            pool.Initialize(transform);
            poolDictionary.Add(pool.Prefab, pool);
            if (pool.Prefab.GetComponent<NetworkIdentity>() != null)
            {
                NetworkClient.RegisterPrefab(pool.Prefab);
            }
            else
            {
                //Debug.LogError("Prefab " + pool.Prefab.name + " does not have a NetworkIdentity and cannot be registered.");
            }
        }
    }


    [Command(requiresAuthority = false)]
    public void CmdSpawnFromPool(string prefabName, Vector3 position, Quaternion rotation)
    {
        // Look up the prefab in the pool dictionary based on its name
        GameObject prefab = null;
        foreach (var key in poolDictionary.Keys)
        {
            if (key.name == prefabName)
            {
                prefab = key;
                break;
            }
        }

        if (prefab == null)
        {
            Debug.LogError("Prefab not found: " + prefabName);
            return;
        }

        GameObject spawnedObject = SpawnFromPool(prefab, position, rotation);
        NetworkServer.Spawn(spawnedObject);
    }


    public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (poolDictionary.TryGetValue(prefab, out Pool pool))
        {
            return pool.Spawn(position, rotation);
        }

        Debug.LogError("Pool not found for prefab " + prefab.name);
        return null;
    }

    [Command(requiresAuthority = false)]
    public void CmdReturnToPool(uint instanceNetId)
    {
        // Look up the instance by its netId
        NetworkIdentity networkIdentity;
        if (NetworkServer.spawned.TryGetValue(instanceNetId, out networkIdentity))
        {
            ReturnToPool(networkIdentity.gameObject);
        }
        else
        {
            Debug.LogError("Instance not found for netId: " + instanceNetId);
        }
    }


    public void ReturnToPool(GameObject instance)
    {
        foreach (Pool pool in pools)
        {
            if (pool.Contains(instance))
            {
                pool.Return(instance);
                return;
            }
        }

        Debug.LogError("Pool not found for instance " + instance.name);
    }
}

[System.Serializable]
public class Pool
{
    public GameObject Prefab;
    public int Size;

    private Queue<GameObject> objects;
    private Transform parentTransform;

    public void Initialize(Transform parentTransform)
    {
        this.parentTransform = parentTransform;
        objects = new Queue<GameObject>();

        for (int i = 0; i < Size; i++)
        {
            GameObject obj = GameObject.Instantiate(Prefab, parentTransform);
            obj.SetActive(false);
            objects.Enqueue(obj);
        }
    }

    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        if (objects.Count == 0) return null;

        GameObject objToSpawn = objects.Dequeue();
        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;

        return objToSpawn;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = parentTransform;
        objects.Enqueue(obj);
    }

    public bool Contains(GameObject obj)
    {
        return obj.name.StartsWith(Prefab.name);
    }
}
