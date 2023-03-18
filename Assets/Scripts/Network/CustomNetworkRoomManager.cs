using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomNetworkRoomManager : NetworkRoomManager
{
    public List<CustomNetworkRoomPlayer> RoomPlayers = new List<CustomNetworkRoomPlayer>();

    public bool IsGameInProgress = false;

    public void HostLobby()
    {
        StartHost();
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        int playerClassIndex = roomPlayer.GetComponent<CustomNetworkRoomPlayer>().CurrentMertIndex;
        var classData = PlayerSkillsDatabase.Instance.GetClassData(playerClassIndex);
        GameObject prefab = classData.ClassPrefab;
        GameObject gamePlayer = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        return gamePlayer;
    }
    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
    {
        CustomNetworkRoomPlayer roomPlayer = (CustomNetworkRoomPlayer)Instantiate(roomPlayerPrefab);
        roomPlayer.SetPlayerData(conn.connectionId);
        return roomPlayer.gameObject;
    }
}
