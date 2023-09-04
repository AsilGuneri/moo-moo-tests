using Mirror;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomNetworkRoomPlayer : NetworkRoomPlayer
{
    [SyncVar] private int connectionId;


    [SerializeField] private RectTransform playerUITransform;

    private CustomNetworkRoomManager CustomManager;
    private RoomPlayerUI roomPlayerUI;

    public int ConnectionId { get { return connectionId; } private set { connectionId = value; } }
    public RoomPlayerUI RoomPlayerUI { get { return roomPlayerUI; } }


    private void Awake()
    {
        CustomManager = NetworkRoomManager.singleton as CustomNetworkRoomManager;
    }

    #region Network Overrides
    public override void OnStartClient()
    {
        base.OnStartClient();
        //For Everyone
        playerUITransform.SetParent(LobbyManager.Instance.RoomPlayerParent);
        CustomManager.RoomPlayers.Add(this);        
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        CustomManager.RoomPlayers.Remove(this);
        if (playerUITransform != null)
        {
            Destroy(playerUITransform.gameObject);
        }

    }
    #endregion

    public void ToggleReadyButton()
    {
        bool readyState = !readyToBegin;
        if (NetworkClient.active && isLocalPlayer)
        {
            CmdChangeReadyState(readyState);
        }
    }
    public void SetPlayerData(int connectionId)
    {
        ConnectionId = connectionId;
        //playerName = ConnectionId.ToString();
    }
    //Has a reference on Next/Previous buttons
  
}