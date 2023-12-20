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
    public void SpawnTestWave()
    {
        SpawnWave(AllWavesData.Instance.TestWave);
        GameFlowManager.Instance.SetGameState(GameState.WaveStarted);
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
        Vector3 currentPosition = initialSpawnPos;

        for (int i = 0; i < waveData.WaveEnemies.Count; i++)
        {
            var subWave = waveData.WaveEnemies[i];

            // SubWave için toplam genişliği hesapla
            float totalWidth = (subWave.Count - 1) * subWave.SpacingX;

            // İlk düşmanın başlangıç pozisyonunu hesapla
            currentPosition.x = initialSpawnPos.x - totalWidth / 2;

            for (int j = 0; j < subWave.Count; j++)
            {
                var obj = PrefabPoolManager.Instance.GetFromPool(subWave.Prefab, currentPosition, Quaternion.identity);
                NetworkServer.Spawn(obj);

                currentPosition.x += subWave.SpacingX; // Her düşman için X koordinatını artır
            }

            // Sıradaki subWave için Z koordinatını güncelle
            currentPosition.x = initialSpawnPos.x;
            currentPosition.z += subWave.SpacingZ;
        }
    }


}

