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

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if(SceneManager.GetActiveScene().name != "SteamLobby")
            return;

        PlayerObjectController gamePlayerInstance = Instantiate(gamePlayerPrefab);
        gamePlayerInstance.conn = conn;
        gamePlayerInstance.connectionID = conn.connectionId;
        gamePlayerInstance.playerIDNumber = gamePlayers.Count + 1;
        gamePlayerInstance.playerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.instance.currentLobbyID, gamePlayers.Count);

        NetworkServer.AddPlayerForConnection(conn, gamePlayerInstance.gameObject);
    }

    public void StartGame(string sceneName){
        ServerChangeScene(sceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if(SceneManager.GetActiveScene().name == "GameScene"){
            for (int i = 0; i < gamePlayers.Count; i++)
            {
                GameObject p = Instantiate(prefab, null);
                NetworkServer.Spawn(p, gamePlayers[i].conn);
                NetworkServer.ReplacePlayerForConnection(gamePlayers[i].conn, p);
            }
        }
        base.OnServerSceneChanged(sceneName);
    }

}
