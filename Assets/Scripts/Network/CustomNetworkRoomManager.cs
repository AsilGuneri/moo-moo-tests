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

    public List<CustomNetworkRoomPlayer> RoomPlayers = new List<CustomNetworkRoomPlayer>();

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

    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
    {
        return null;//Instantiate(roomPlayerPrefab, LobbyManager.Instance.RoomPlayerParent.transform).gameObject;
    }

    public override void OnStopServer()
    {
        RoomPlayers.Clear();
        IsGameInProgress = false;
    }


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        if (!IsGameInProgress)
        {
            var player = conn.identity.GetComponent<CustomNetworkRoomPlayer>();
            return;
        }
    }
}
