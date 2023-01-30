using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;
using UnityEngine.SceneManagement;
using Steamworks;

public class CustomNetworkManager : NetworkManager
{ 

    public static event Action ClientOnConnected;
    public static event Action ClientOnDisonnected;
    public static event Action HostOnStop;

    private bool isGameInProgress = false;

    public List<CustomNetworkPlayer> players { get; } = new List<CustomNetworkPlayer>();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        
        if(SceneManager.GetActiveScene().name == "SteamLobby"){
            CustomNetworkPlayer player = conn.identity.GetComponent<CustomNetworkPlayer>();

            player.connectionID = conn.connectionId;
            player.playerIdNumber = players.Count + 1;
            player.playerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.instance.currentLobbyID, players.Count);
        }

    }

    public void StartGame(){
        #if !UNITY_EDITOR
        if(players.Count < 2)
            return;
        #endif

        isGameInProgress = true;

        ServerChangeScene("GameScene");
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if(SceneManager.GetActiveScene().name == "GameScene"){
            
            foreach(CustomNetworkPlayer p in players){
                p.GetComponent<PlayerMertController>().Activate();
            }
        }

        base.OnServerSceneChanged(sceneName);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        ClientOnConnected?.Invoke();
    }


    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        ClientOnDisonnected?.Invoke();
    }

    public override void OnStartClient()
    {
    }

    public override void OnStopClient()
    {
        LobbyController.instance.UpdatePlayerList();
        players.Clear();
        base.OnStopClient();
    }
}
