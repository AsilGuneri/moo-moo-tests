using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Steamworks;

public class CustomNetworkPlayer : NetworkBehaviour
{
    [SyncVar]public int connectionID;
    [SyncVar]public int playerIdNumber;
    [SyncVar]public ulong playerSteamID;

    [SyncVar(hook = nameof(PlayerNameUpdate))]
    public string playerName;

    [SyncVar]
    private bool isPartyOwner = false;

    private CustomNetworkManager _manager;
    private CustomNetworkManager manager{
        get{
            if(_manager != null)
                return _manager;
            return _manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        LobbyController.instance.FindLocalPlayer();
        LobbyController.instance.UpdateLobbyName();
    }

    public override void OnStartClient()
    {
        manager.players.Add(this);
        LobbyController.instance.UpdateLobbyName();
        LobbyController.instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        manager.players.Remove(this);
        LobbyController.instance.UpdatePlayerList();
    }

    [Command]
    private void CmdSetPlayerName(string playerName){
        this.PlayerNameUpdate(this.playerName, playerName);
    }

    public void PlayerNameUpdate(string oldValue, string newValue){
        if(isServer){
            this.playerName = newValue;
        }
        else if(isClient){
            LobbyController.instance.UpdatePlayerList();
        }
    }

    [Command]
    public void CmdStartGame(){
        if(!isPartyOwner)
            return;

        ((CustomNetworkManager)NetworkManager.singleton).StartGame();

    }
    

}
