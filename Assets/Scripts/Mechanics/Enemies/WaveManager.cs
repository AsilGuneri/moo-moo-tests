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

        //GoldManager.Instance.DistributeGold(AllWavesData.Instance.WavesData[currentWaveIndex].WaveGoldReward);

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
        if (readyCount >= CustomNetworkRoomManager.singleton.numPlayers)
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

