using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using System;
using Mirror;
using UnityEngine.UI;
using MyBox;
using TMPro;

public class WaveManager : NetworkSingleton<WaveManager>
{
    public int CurrentWaveIndex { get => currentWaveIndex; }

    [SerializeField] Transform spawnArea;
    [SerializeField] float spacing = 2f;

    

    Vector3 initialSpawnPos;
    int currentWaveIndex = 0;

    private void Start()
    {
        initialSpawnPos = spawnArea.position;
    }
    [Server]
    public void Spawn(int countdown = 0)
    {
        if (countdown != 0)
            StartCoroutine(SpawnWithCountdown(countdown));
        else
            SpawnNextWave();
    }
    [ServerCallback]
    public void OnWaveEnd()
    {
        int maxWaveIndex = AllWavesData.Instance.WavesData.Count - 1;

        //GoldManager.Instance.DistributeGold(AllWavesData.Instance.WavesData[currentWaveIndex].WaveGoldReward);

        currentWaveIndex++;

        if (currentWaveIndex > maxWaveIndex)
        {
            currentWaveIndex = maxWaveIndex;
        }
        GameFlowManager.Instance.SetGameState(GameState.Free);
    }

  

    [Server]
    private void SpawnNextWave()
    {
        if (currentWaveIndex > AllWavesData.Instance.WavesData.Count - 1)
        {
            currentWaveIndex = AllWavesData.Instance.WavesData.Count - 1;
        }
        SpawnWave(AllWavesData.Instance.WavesData[currentWaveIndex]);
        GameFlowManager.Instance.SetGameState(GameState.WaveStarted);
    }

    [Server]
    private IEnumerator SpawnWithCountdown(int seconds)
    {
        NotificationManager.Instance.SetNotification("NEXT WAVE IN :", seconds);
        yield return Extensions.GetWait(seconds);
        SpawnNextWave();
    }
    private void SpawnWave(WaveData waveData)
    {
        Vector3 currentPosition = initialSpawnPos; // Starting from the initial position.

        for (int i = 0; i < waveData.SubWaves.Count; i++)
        {
            var subWave = waveData.SubWaves[i];

            for (int j = 0; j < subWave.Count; j++)
            {
                var obj = PrefabPoolManager.Instance.GetFromPool(subWave.Prefab, currentPosition, Quaternion.identity);
                NetworkServer.Spawn(obj);

                // Move to the next spawn position in the row.
                currentPosition.x += subWave.SpacingX;
            }

            // Once the subwave is done, reset x position and move 'backwards' in preparation for the next subwave.
            currentPosition.x = initialSpawnPos.x;
            currentPosition.z += subWave.SpacingZ;
        }
    }

}

