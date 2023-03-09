using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNetworkRoomPlayer : NetworkRoomPlayer
{
    CustomNetworkRoomManager CustomManager;

    private void Awake()
    {
        CustomManager = NetworkRoomManager.singleton as CustomNetworkRoomManager;
    }
    public override void OnStartClient()
    {
        if (NetworkServer.active) return;
        base.OnStartClient();
        CustomManager.RoomPlayers.Add(this);
    }
    public override void OnStopClient()
    {
        if (!isClientOnly) return;
        base.OnStopClient();
        CustomManager.RoomPlayers.Remove(this);

    }
}
