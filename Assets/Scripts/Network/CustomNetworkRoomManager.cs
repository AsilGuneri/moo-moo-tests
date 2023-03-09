using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomNetworkRoomManager : NetworkRoomManager
{
    //geçiciler
    public GameObject PlayerPrefab;
    public GameObject PlayerPrefab2;
    public bool purple;
    //olmayanlar

    public List<CustomNetworkRoomPlayer> RoomPlayers { get; } = new List<CustomNetworkRoomPlayer>();


    public bool IsGameInProgress = false;

    public void HostLobby()
    {
        StartHost();
    }
    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        GameObject prefab = purple ? PlayerPrefab2 : PlayerPrefab;
        GameObject gamePlayer = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        return gamePlayer;
    }
    public override void OnStopServer()
    {
        RoomPlayers.Clear();
        IsGameInProgress = false;
    }
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        if (!IsGameInProgress)
        {
            var player = conn.identity.GetComponent<CustomNetworkRoomPlayer>();
            RoomPlayers.Remove(player);
            return;
        }
    }
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        if (!IsGameInProgress)
        {
            var player = conn.identity.GetComponent<CustomNetworkRoomPlayer>();
            RoomPlayers.Add(player);
            return;
        }
    }
    public override void OnStopClient()
    {
        base.OnStopClient();
        RoomPlayers.Clear();
    }
}
