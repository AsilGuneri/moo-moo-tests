using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    public static LobbyManager Instance { get; private set; }

    [SerializeField] private GameObject roomPlayerPrefab;
    [SerializeField] private NetworkIdentity roomPlayerParent;

    public NetworkIdentity RoomPlayerParent
    {
        get => roomPlayerParent;
    }

    private CustomNetworkRoomManager CustomManager;

    private void Awake()
    {
        Instance = this;
        CustomManager = NetworkRoomManager.singleton as CustomNetworkRoomManager;
    }

    public void StartGame()
    {
        CustomManager.ServerChangeScene(CustomManager.GameplayScene);
        CustomManager.IsGameInProgress = true;
    }

    public void LeaveLobby()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            CustomManager.StopHost();
        }
        else
        {
            CustomManager.StopClient();
        }
        SceneManager.LoadScene(0);
    }

    public void UpdatePlayerItems()
    {
        //foreach (var player in CustomManager.RoomPlayers)
        //{
        //    player.CmdSetParent(RoomPlayerParent);
        //}
    }
}
