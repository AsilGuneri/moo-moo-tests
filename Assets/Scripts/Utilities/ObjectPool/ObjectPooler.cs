using System.Collections.Generic;
using UnityEngine;
using System;
using Utilities;

[CreateAssetMenu(fileName = "ObjectPooler", menuName = "Scriptable Objects/Object Pooler", order = 1)]
public class ObjectPooler : Singleton<ObjectPooler>
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    public void Initialize()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                 if (obj.TryGetComponent<PoolObject>(out PoolObject poolObject))
                {
                    poolObject.PoolTag = pool.tag;
                }
                else
                {
                    var poolObj = obj.AddComponent<PoolObject>();
                    poolObj.PoolTag = pool.tag;
                }
                objectPool.Enqueue(obj);
               
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        if (poolDictionary[tag].Count == 0)
        {
            Debug.LogError("No objects remaining in pool with tag " + tag);
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }

    public GameObject SpawnFromPoolWithPrefab(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        string tag = string.Empty;

        foreach (Pool pool in pools)
        {
            if (pool.prefab == prefab)
            {
                tag = pool.tag;
                break;
            }
        }

        if (string.IsNullOrEmpty(tag))
        {
            Debug.LogWarning("No pool found containing the given prefab.");
            return null;
        }

        return SpawnFromPool(tag, position, rotation);
    }
    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError("Pool with tag " + tag + " doesn't exist.");
            return;
        }

        objectToReturn.SetActive(false);
        poolDictionary[tag].Enqueue(objectToReturn);
    }

}