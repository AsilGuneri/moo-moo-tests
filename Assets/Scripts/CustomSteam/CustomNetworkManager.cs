using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using Steamworks;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField]private GameObject prefab;
    [SerializeField]private PlayerObjectController gamePlayerPrefab;
    public List<PlayerObjectController> gamePlayers { get; } = new List<PlayerObjectController>();
    NetworkConnectionToClient _conn;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if(SceneManager.GetActiveScene().name != "SteamLobby")
            return;

        PlayerObjectController gamePlayerInstance = Instantiate(gamePlayerPrefab);
        gamePlayerInstance.connectionID = conn.connectionId;
        gamePlayerInstance.playerIDNumber = gamePlayers.Count + 1;
        gamePlayerInstance.playerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.instance.currentLobbyID, gamePlayers.Count);

        NetworkServer.AddPlayerForConnection(conn, gamePlayerInstance.gameObject);
        _conn = conn;
    }

    public void StartGame(string sceneName){
        ServerChangeScene(sceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if(SceneManager.GetActiveScene().name == "GameScene"){
            GameObject p = Instantiate(prefab, null);
            NetworkServer.Spawn(p, _conn);
        }
        base.OnServerSceneChanged(sceneName);
    }

}
