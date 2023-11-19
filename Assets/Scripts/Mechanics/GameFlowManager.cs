using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class GameFlowManager : NetworkSingleton<GameFlowManager>
{
    public GameState CurrentState { get => currentState; }
    public int RespawnTime = 5;

    [SerializeField] int firstWaveCountdown;
    [SerializeField] int waveCountdown;
    [SerializeField] Button readyButton;
    int readyCount;


    GameState currentState;

    public void SetGameState(GameState state)
    {
        currentState = state;
        switch (state)
        {
            case GameState.Free:
                OnFree();
                break;
            case GameState.WaveCountdown:
                var countdown = WaveManager.Instance.CurrentWaveIndex == 0 ? firstWaveCountdown : waveCountdown;
                OnWaveCountdown(countdown);
                break;
            case GameState.WaveStarted:
                OnWaveStart();
                break;
            case GameState.GameEnd:
                OnGameEnd();
                break;
        }
    }

    [Server]
    private void OnGameEnd()
    {
        var manager = (CustomNetworkRoomManager)NetworkRoomManager.singleton;
        EndGameManager.Instance.OnGameEnd();
      //  manager.StopHost();
    }

    /// <summary>
    /// Everyone in the lobby loaded
    /// </summary>
    [Server]
    public void OnGameStart()
    {
        StartCoroutine(StartGameRoutine());
    }

    IEnumerator StartGameRoutine()
    {
        yield return Extensions.GetWait(1);
        //SetGameState(GameState.WaveCountdown);//comment it to test waves manually

    }
    private void OnFree()
    {
        SetReadyButton();
    }

    private void OnWaveCountdown(int countdown)
    {
        WaveManager.Instance.Spawn(countdown);
    }
    private void OnWaveStart()
    {

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
    WaveStarted,
    GameEnd
}