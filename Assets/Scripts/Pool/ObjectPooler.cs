using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;
using UnityEngine.AI;
using Mirror;

public class ObjectPooler : Singleton<ObjectPooler>
{
    [SerializeField] private Pool[] allPools;

    private List<PooledObject> allPooledObjects = new List<PooledObject>();
    private void Awake()
    {
        InstantiatePoolObjects();
    }

    public GameObject Spawn(string poolName, Vector3 position, Quaternion rotation = new Quaternion())
    {
        var pool = 0;
        for (int i = 0; i < allPooledObjects.Count; i++)
        {
            if(poolName == allPooledObjects[i].PoolName)
            {
                pool = i;
                break;
            }
        }
        foreach (var poolObj in allPooledObjects[pool].PooledObjects.Where(poolObj => !poolObj.activeSelf))
        {
            // Set active:
            poolObj.SetActive(true);
            poolObj.transform.localPosition = position;
            poolObj.transform.localRotation = rotation;
            if (poolObj.TryGetComponent(out NavMeshAgent agent)) agent.enabled = true;

            return poolObj;
        }
        GameObject obj = Instantiate(allPools[pool].Prefab, position, rotation);
        allPooledObjects[pool].PooledObjects.Add(obj);
        return obj;
    }
    private void InstantiatePoolObjects()
    {
        for (int i = 0; i < allPools.Length; i++)
        {
            List<GameObject> pooledObjects = new List<GameObject>();
            GameObject poolParent = new GameObject(allPools[i].PoolName);
            for (int k = 0; k < allPools[i].StartingQuantity; k++)
            {
                GameObject obj = Instantiate(allPools[i].Prefab, poolParent.transform);
                pooledObjects.Add(obj);
                obj.SetActive(false);
                NetworkServer.Spawn(obj);

            }


            PooledObject pooledObject = new PooledObject(allPools[i].PoolName, pooledObjects);
            allPooledObjects.Add(pooledObject);
        }
    }

}
[Serializable]
public class PooledObject
{
    public string PoolName;
    public List<GameObject> PooledObjects;

    public PooledObject(string poolName, List<GameObject> pooledObjects)
    {
        PoolName = poolName;
        PooledObjects = pooledObjects;
    }
}