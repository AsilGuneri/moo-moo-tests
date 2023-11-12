using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

public class EndGameManager : NetworkSingleton<EndGameManager> 
{
    [SerializeField] GameObject statsPanelParent;
    [SerializeField] Transform statsContentParent;
    [SerializeField] GameObject playerStatsPrefab;
    [SerializeField] Button returnButton;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameFlowManager.Instance.SetGameState(GameState.GameEnd);
        }
    }
    [Server]
    public void OnGameEnd()
    {
        RpcGameEnd();
    }
    [ClientRpc]
    private void RpcGameEnd()
    {
        DisplayStats();
    }

    private void DisplayStats()
    {
        returnButton.onClick.AddListener(ReturnToOfflineScene);
        statsPanelParent.SetActive(true);
        Extensions.DestroyAllChildren(statsContentParent);
        var manager = (CustomNetworkRoomManager)NetworkRoomManager.singleton;
        var players = manager.GamePlayers;
        foreach( var player in players)
        {
            var playerStatsObj = Instantiate(playerStatsPrefab, statsContentParent);
            var slot = playerStatsObj.GetComponent<EndGameStatSlot>();
            slot.Setup(player.PlayerName);

        }
    }
    private void ReturnToOfflineScene()
    {
        var manager = (CustomNetworkRoomManager)CustomNetworkRoomManager.singleton;
        if (isServer) manager.StopHost();
        else manager.StopClient();
    }
}
