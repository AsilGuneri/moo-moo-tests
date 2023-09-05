using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using System;
using Mirror;
using UnityEngine.UI;

public class WaveManager : NetworkSingleton<WaveManager>
{
    [SerializeField] Transform spawnArea;
    [SerializeField] float spacing = 2f;
    [SerializeField] Button readyButton;
    Vector3 initialSpawnPos;
    int lastSpawnedIndex = -1;
    int readyCount;
    int currentWaveIndex = 0;

    private void Start()
    {
        initialSpawnPos = spawnArea.position;
    }
    [Server]
    public void SpawnTestWave()
    {
        SpawnWave(AllWavesData.Instance.WavesData[0]);
    }
    [ServerCallback]
    public void OnWaveEnd()
    {
        int maxWaveIndex = AllWavesData.Instance.WavesData.Count - 1;

        GoldManager.Instance.DistributeGold(AllWavesData.Instance.WavesData[currentWaveIndex].WaveGoldReward);

        currentWaveIndex++;

        if (currentWaveIndex > maxWaveIndex)
        {
            currentWaveIndex = maxWaveIndex;
        }

        StartVote();
    }

    [ClientRpc]
    private void StartVote()
    {
        readyCount = 0;
        readyButton.interactable = true;
        readyButton.gameObject.SetActive(true);
        readyButton.onClick.RemoveAllListeners();
        readyButton.onClick.AddListener(() =>
        {
            readyButton.interactable = false;
            Vote();
        });
    }

    [Command(requiresAuthority = false)]
    private void Vote()
    {
        readyCount++;
        CheckVotes();
    }

    [Server]
    private void CheckVotes()
    {
        if (readyCount >= CustomNetworkManager.singleton.numPlayers)
        {
            SpawnNextWave();
        }
    }

    [ServerCallback]
    private void SpawnNextWave()
    {
        if (currentWaveIndex > AllWavesData.Instance.WavesData.Count - 1)
        {
            currentWaveIndex = AllWavesData.Instance.WavesData.Count - 1;
        }
        SpawnWave(AllWavesData.Instance.WavesData[currentWaveIndex]);
    }


    [ServerCallback]
    private void SpawnWave(WaveData waveData)
    {

        foreach (SubWave subWave in waveData.SubWaves)
        {
            Vector3 position = spawnArea.position;
            Debug.Log("asilxx1 " + subWave.Prefab.name);
            PrefabPoolManager.Instance.SpawnFromPoolServer(subWave.Prefab, position, Quaternion.identity);
        }
    }
}

