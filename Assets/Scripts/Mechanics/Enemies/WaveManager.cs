using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using System;
using Mirror;
using UnityEngine.UI;
using Mirror.Examples;

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

    private void SpawnWave(WaveData waveData)
    {
        spawnArea.position = initialSpawnPos;
        Vector3 offset = Vector3.zero;
        int maxRows = 0;

        foreach (SubWave subWave in waveData.SubWaves)
        {
            int columnsPerRow = Mathf.CeilToInt((float)subWave.Count / subWave.Columns);
            int middleRowIndex = columnsPerRow / 2;
            int middleColumnIndex = subWave.Columns / 2;

            for (int column = 0; column < subWave.Columns; column++)
            {
                int offsetColumnIndex = column - middleColumnIndex;

                for (int row = 0; row < columnsPerRow; row++)
                {
                    int index = row * subWave.Columns + column;
                    if (index >= subWave.Count)
                        break;

                    int offsetRowIndex = row - middleRowIndex;

                    offset.x = offsetColumnIndex * spacing;
                    offset.z = offsetRowIndex * spacing;

                    Vector3 position = spawnArea.position + offset;

                    GameObject enemy = ObjectPooler.Instance.Get(waveData.SubWaves[0].Prefab, position, Quaternion.identity);
                    NetworkServer.Spawn(enemy);
                }
            }

            maxRows += columnsPerRow;
            spawnArea.position -= Vector3.forward * spacing * (maxRows + 1);
        }
        lastSpawnedIndex++;
    }
}

