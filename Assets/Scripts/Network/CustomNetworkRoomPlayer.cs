using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomNetworkRoomPlayer : NetworkRoomPlayer
{
    [SyncVar] private int connectionId;
    [SyncVar(hook = nameof(UpdatePlayerName))] private string playerName;

    public int ConnectionId { get { return connectionId; } private set { connectionId = value; } }

    private CustomNetworkRoomManager CustomManager;

    [SerializeField] private Button[] selectionButtons;
    [SerializeField] private TextMeshProUGUI nameText;


    private void Awake()
    {
        CustomManager = NetworkRoomManager.singleton as CustomNetworkRoomManager;
    }
    private void OnEnable()
    {
        DisableSelectionButtons();
        EnableSelectionButtons();
        transform.SetParent(LobbyManager.Instance.RoomPlayerParent);

    }

    #region Network
    public override void OnStartClient()
    {
        base.OnStartClient();
        CustomManager.RoomPlayers.Add(this);
    }
    public override void OnStopClient()
    {
        base.OnStopClient();
        CustomManager.RoomPlayers.Remove(this);

    }
    #endregion

    public void SetPlayerData(int connectionId)
    {
        ConnectionId = connectionId;
        playerName = ConnectionId.ToString();
    }

    private void DisableSelectionButtons()
    {
        foreach (var button in selectionButtons)
        {
            if (button.interactable)
            {
                button.interactable = false;
            }
        }
    }

    private void EnableSelectionButtons()
    {
        if (hasAuthority)
        {
            foreach (var button in selectionButtons)
            {
                button.interactable = true;
            }
        }

    }

    private void UpdatePlayerName(string oldValue, string newValue)
    {
        nameText.text = newValue;
    }
}
