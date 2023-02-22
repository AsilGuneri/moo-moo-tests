using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/Wave Data", order = 1)]

public class WaveData : ScriptableObject
{
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

