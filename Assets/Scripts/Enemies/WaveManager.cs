using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using System;
using Mirror;

public class WaveManager : Singleton<WaveManager>//
{
    public Wave[] waves;
    [SerializeField] private SpawnPoint[] spawnPoints;

    private int _currentWave = 0;

    public void SpawnWave(Wave nextWave)
    {
        float minionPerLine = Mathf.Sqrt(nextWave.minionPerPoint);
        
        foreach (SpawnPoint point in spawnPoints)
        {
            //setup point, point e vector3 listesi aç içini doldur spawn spaceleri kaþele, spawn spaceler deðiþmedikçe tekrar setup ý çaðýrma
            float spawnSpaceZ = point.isHorizontal ? nextWave.spawnSpace.x : nextWave.spawnSpace.z;
            float spawnSpaceX = point.isHorizontal ? nextWave.spawnSpace.z : nextWave.spawnSpace.x;

                for (int j = 0; j < minionPerLine; j++)
                {
                    float spawnZPos = (point.transform.position.z - ((minionPerLine -1) * spawnSpaceZ / 2)) + (j * nextWave.spawnSpace.z);
                    for (int k = 0; k < minionPerLine; k++)
                    {
                        float spawnXPos = (point.transform.position.x - ((minionPerLine - 1) * spawnSpaceX / 2)) + (k * nextWave.spawnSpace.x);
                        Vector3 spawnPoint = new Vector3(spawnXPos, nextWave.spawnSpace.y, spawnZPos);
                    //ObjectPooler.Instance.Spawn(nextWave.prefab.name, spawnPoint, Quaternion.identity); ///TODO: Use pool
                    var obj = Instantiate(nextWave.prefab, spawnPoint, Quaternion.identity);
                    obj.GetComponent<BasicEnemyController>().Activate();
                    NetworkServer.Spawn(obj);
                    UnitManager.Instance.RegisterUnit(obj, UnitType.WaveEnemy);
                    }

                }
            
        }
    }
    public void SpawnNextWave()
    {
        _currentWave++;
        SpawnWave(waves[_currentWave]);
    }
}
[Serializable]
public class Wave
{
    public int waveIndex;
    public GameObject prefab;
    public int minionPerPoint;
    public Vector3 spawnSpace;
    public bool isHorizontal;
    
}

