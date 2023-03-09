using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : NetworkBehaviour
{
    public static RoomManager Instance { get; private set; }

    private CustomNetworkRoomManager CustomNetworkRoomManager;

    private void Awake()
    {
        Instance = this;
        CustomNetworkRoomManager = NetworkRoomManager.singleton as CustomNetworkRoomManager;
    }

    public void LeaveLobby()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            CustomNetworkRoomManager.StopHost();
        }
        else
        {
            CustomNetworkRoomManager.StopClient();
        }
        SceneManager.LoadScene(0);
    }
}
