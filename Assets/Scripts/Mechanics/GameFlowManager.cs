using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class GameFlowManager : NetworkSingleton<GameFlowManager>
{
    [SerializeField] int firstWaveCountdown;
    /// <summary>
    /// Everyone in the lobby loaded
    /// </summary>
    [Server]
    public void OnGameStart()
    {
        WaveManager.Instance.Spawn(firstWaveCountdown);
    }
}
