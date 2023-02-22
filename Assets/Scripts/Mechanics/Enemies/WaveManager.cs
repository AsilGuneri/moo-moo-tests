using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using System;
using Mirror;

public class WaveManager : NetworkSingleton<WaveManager>
{
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // Set the Gizmo color to red
        Gizmos.color = Color.red;
        // Draw a wire sphere at the GameObject's position
        Gizmos.DrawSphere(transform.position, 1);
    }
#endif
    public List<WaveData> WavesData = new List<WaveData>();
    public Transform spawnArea;
    public float spacing = 2f;
    private Vector3 initialSpawnPos;

    private void Start()
    {
        initialSpawnPos = spawnArea.position;
    }

    public void TestWaveSpawn()
    {
        SpawnWave(WavesData[0]);
    }

    public void SpawnWave(WaveData waveData)
    {
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
        spawnArea.position = initialSpawnPos;
    }

}

