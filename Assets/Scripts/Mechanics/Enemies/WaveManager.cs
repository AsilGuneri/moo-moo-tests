using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using System;
using Mirror;
using UnityEngine.UI;

public class WaveManager : NetworkSingleton<WaveManager>
{
    [SerializeField] List<WaveData> WavesData = new List<WaveData>();
    [SerializeField] Transform spawnArea;
    [SerializeField] float spacing = 2f;
    [SerializeField] Button readyButton;
    Vector3 initialSpawnPos;
    int lastSpawnedIndex = -1;
    [SyncVar] int readyCount;

    private void Start()
    {
        initialSpawnPos = spawnArea.position;
    }

    public void TestWaveSpawn()
    {
        SpawnWave(WavesData[0]);
    }

    public void OnWaveEnd()
    {
        StartVote();
    }
    
    private void StartVote()
    {
        readyCount = 0;
        readyButton.interactable = true;
        readyButton.gameObject.SetActive(true);
        readyButton.onClick.RemoveAllListeners();
        readyButton.onClick.AddListener(() =>
        {
        readyButton.interactable = false;
        readyCount++;
            if (readyCount >= CustomNetworkManager.singleton.numPlayers)
                TestWaveSpawn();
        });
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

                    var enemy = Instantiate(subWave.Prefab, position, Quaternion.identity);
                    NetworkServer.Spawn(enemy);
                }
            }

            maxRows += columnsPerRow;
            spawnArea.position -= Vector3.forward * spacing * (maxRows + 1);
        }
        lastSpawnedIndex++;
    }
}

