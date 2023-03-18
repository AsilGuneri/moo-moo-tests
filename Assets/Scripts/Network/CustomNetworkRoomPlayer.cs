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
    [SerializeField] private Button[] selectionButtons;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Image classImage;
    [SerializeField] private TextMeshProUGUI classNameText;
    [SerializeField] private Button readyButton;
    [SerializeField] private TextMeshProUGUI readyButtonText;

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
        roomPlayerUI = GetComponent<RoomPlayerUI>();
        //
        CustomManager.RoomPlayers.Add(this);
        roomPlayerUI.InitializeUI(hasAuthority, connectionId);
        if (!hasAuthority) //For Everyone but owner
        {
            roomPlayerUI.CmdRefreshPlayerUI();
        }
        else //For Owner
        {
            roomPlayerUI.CmdSetCurrentIndex(0);
            //name variable should be syncvar and set here.
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        CustomManager.RoomPlayers.Remove(this);
        if (playerUITransform.gameObject != null)
        {
            Destroy(playerUITransform.gameObject);
        }

    }
    #endregion

    public void ToggleReadyButton()
    {
        if (NetworkClient.active && isLocalPlayer)
        {

            if (readyToBegin)
            {
                CmdChangeReadyState(false);
                roomPlayerUI.OnSetReady(false);
            }
            else
            {
                CmdChangeReadyState(true);
                roomPlayerUI.OnSetReady(true);

            }
        }
    }
    public void SetPlayerData(int connectionId)
    {
        ConnectionId = connectionId;
        //playerName = ConnectionId.ToString();
    }
    //Has a reference on Next/Previous buttons
  
}