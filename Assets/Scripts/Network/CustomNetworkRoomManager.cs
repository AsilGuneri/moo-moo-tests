using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNetworkRoomManager : NetworkRoomManager
{
    //geçiciler
    public GameObject PlayerPrefab;
    public GameObject PlayerPrefab2;
    public bool purple;


    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        GameObject prefab = purple ? PlayerPrefab2 : PlayerPrefab;
        GameObject gamePlayer = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        return gamePlayer;
    }
}
