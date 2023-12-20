using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/Managers/Wave Data", order = 1)]

public class AllWavesData : ScriptableSingleton<AllWavesData>
{
    public WaveData TestWave;
    public List<WaveData> WavesData = new List<WaveData>();
}
[Serializable]
public class WaveData
{
    public List<WaveEnemyData> WaveEnemies = new List<WaveEnemyData>();
}
[Serializable]
public class WaveEnemyData
{
    public GameObject Prefab;
    public int Count;
    public float SpacingX = 1f;
    public float SpacingZ = 1.5f;
}

