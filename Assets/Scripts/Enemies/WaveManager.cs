using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using System;
using Mirror;

public class WaveManager : NetworkSingleton<WaveManager>
{
    [SerializeField] public Wave[] waves;
    [SerializeField] private SpawnPoint[] spawnPoints;


    [SyncVar] private int _currentWave = 0;
    private Wave _waveToSpawn;

    public int CurrentWave
    {
        get => _currentWave;
    }

    [ServerCallback]
    public void SetCurrentWave(int waveIndex)
    {
        _currentWave = waveIndex;
    }

    [ServerCallback]
    public void SpawnWave(Wave nextWave)
    {
        _waveToSpawn = nextWave;
        StartCoroutine(nameof(SpawnWaveRoutine));
    }
    public void OnWaveEnd()
    {

    }
    /*  BURAYA BAKARLAR   */
    /*  BURAYA BAKARLAR   */
    /*  BURAYA BAKARLAR   */
    /*  BURAYA BAKARLAR   */
    /*  BURAYA BAKARLAR   */
    /*  BURAYA BAKARLAR   */
    /*  BURAYA BAKARLAR   */
    /*  BURAYA BAKARLAR   */
    /*  BURAYA BAKARLAR   */
    /*  BURAYA BAKARLAR   */
    /*  BURAYA BAKARLAR   */
    /*  BURAYA BAKARLAR   */
    /*  BURAYA BAKARLAR   */
    /*  BURAYA BAKARLAR   */
    /*  BURAYA BAKARLAR   */
    private IEnumerator SpawnWaveRoutine()
    {
        foreach (SubWave subWave in _waveToSpawn.SubWaves)
        {
            for (int i = 0; i < subWave.Count; i++)
            {
                GameObject minion = Instantiate(subWave.MinionPrefab, spawnPoints[0].transform.position, Quaternion.identity);
                NetworkServer.Spawn(minion);
            }          
            yield return new WaitForSeconds(subWave.AfterDelay);
        }
    }
    private void EndWave()
    {
         SpawnNextWave();
    }
    private void SpawnNextWave()
    {
        _currentWave++;
        SpawnWave(waves[_currentWave]);
    }
    
}
[Serializable]
public class Wave
{
    public int WaveIndex;
    public SubWave[] SubWaves;
    public Vector3 SpawnSpace;
    
}
[Serializable]
public class SubWave
{
    public GameObject MinionPrefab;
    public float Count;
    public float AfterDelay;
}

