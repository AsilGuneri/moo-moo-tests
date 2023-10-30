using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class GameFlowManager : NetworkSingleton<GameFlowManager>
{

    [SerializeField] Button readyButton;
    int readyCount;

    [SerializeField] int firstWaveCountdown;

    GameState currentState;

    public void SetGameState(GameState state, int countdown = 0)
    {
        currentState = state;
        switch (state)
        {
            case GameState.Free:
                OnFree();
                break;
            case GameState.WaveCountdown:
                OnWaveCountdown(countdown);
                break;
            case GameState.WaveStarted:
                break;
        }
    }

    /// <summary>
    /// Everyone in the lobby loaded
    /// </summary>
    [Server]
    public void OnGameStart()
    {
        SetGameState(GameState.WaveCountdown);
    }

    private void OnFree()
    {
        SetReadyButton();
    }

    private void OnWaveCountdown(int countdown)
    {
        WaveManager.Instance.Spawn(countdown);
    }
    [ClientRpc]
    private void SetReadyButton()
    {
        readyCount = 0;
        readyButton.interactable = true;
        readyButton.gameObject.SetActive(true);
        readyButton.onClick.RemoveAllListeners();
        readyButton.onClick.AddListener(() =>
        {
            readyButton.interactable = false;
            SetReady();
        });
    }

    [Command(requiresAuthority = false)]
    private void SetReady()
    {
        readyCount++;
        CheckEveryoneReady();
    }

    [Server]
    private void CheckEveryoneReady()
    {
        if (readyCount >= CustomNetworkRoomManager.singleton.numPlayers)
        {
            GameFlowManager.Instance.SetGameState(GameState.WaveCountdown);
        }
    }
}
public enum GameState
{
    None,
    Free,
    WaveCountdown,
    WaveStarted
}