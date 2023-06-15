using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/Managers/Wave Data", order = 1)]

public class AllWavesData : ScriptableSingleton<AllWavesData>
{
    public List<WaveData> WavesData = new List<WaveData>();
}
[Serializable]
public class WaveData
{
    public int WaveGoldReward;
    public List<SubWave> SubWaves = new List<SubWave>();
}
[Serializable]
public class SubWave
{
    public GameObject Prefab;
    public int Count;
    public float Spacing = 2f;
    public int Columns = 1;
}

