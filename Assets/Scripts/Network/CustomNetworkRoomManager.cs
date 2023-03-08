using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNetworkRoomManager : NetworkRoomManager
{
    //geçiciler
    public GameObject PlayerPrefab;


    private void CreateGamePlayer(NetworkConnectionToClient conn)
    {
        GameObject gamePlayer = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.ReplacePlayerForConnection(conn, gamePlayer, true);
    }
}
