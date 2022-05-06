using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using System;

public class WaveManager : Singleton<WaveManager>//
{
    public Wave[] waves;
    public SpawnPoint[] spawnPoints;

    public void SpawnWave(Wave nextWave)
    {
        float minionPerLine = Mathf.Sqrt(nextWave.minionPerPoint);
        
        foreach (SpawnPoint point in spawnPoints)
        {
            //setup point, point e vector3 listesi a� i�ini doldur spawn spaceleri ka�ele, spawn spaceler de�i�medik�e tekrar setup � �a��rma
            float spawnSpaceZ = point.isHorizontal ? nextWave.spawnSpace.x : nextWave.spawnSpace.z;
            float spawnSpaceX = point.isHorizontal ? nextWave.spawnSpace.z : nextWave.spawnSpace.x;
            for (int i = 0; i < nextWave.minionPerPoint; i++)
            {
                for (int j = 0; j < minionPerLine; j++)
                {
                    float spawnZPos = (point.transform.position.z - ((minionPerLine -1) * spawnSpaceZ / 2)) + (j * nextWave.spawnSpace.z);
                    for (int k = 0; k < minionPerLine; k++)
                    {
                        float spawnXPos = (point.transform.position.x - ((minionPerLine - 1) * spawnSpaceX / 2)) + (k * nextWave.spawnSpace.x);
                        Vector3 spawnPoint = new Vector3(spawnXPos, nextWave.spawnSpace.y, spawnZPos);
                        Instantiate(nextWave.prefab, spawnPoint, Quaternion.identity); ///TODO: Use pool
                    }
                }
            }
        }
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

