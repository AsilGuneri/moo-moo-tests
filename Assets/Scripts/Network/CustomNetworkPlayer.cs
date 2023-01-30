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
    public string playerName = "Missing Name";

    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;

    [SyncVar(hook = nameof(AuthorityHandlePartyOwnerStateUpdated))]
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
    }

    public override void OnStartClient()
    {
        Debug.Log("On Start Client: " + playerName);
        manager.players.Add(this);
        LobbyController.instance.UpdateLobbyName();
        LobbyController.instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        Debug.Log("On Stop Client: " + playerName);
        // manager.players.Remove(this);
        // LobbyController.instance.UpdatePlayerList();
    }

    [Command] //Client calling a method on server
    private void CmdSetPlayerName(string playerName){
        this.PlayerNameUpdate(this.playerName, playerName);
        RpcLogNewName(playerName);
        LobbyController.instance.UpdatePlayerList();
    }

    [ClientRpc] //Server calling a method on all clients
    private void RpcLogNewName(string newDisplayName)
    {
        Debug.Log("Player Name Setted: " + newDisplayName);
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
    
    [Server] 
    public void SetPartyOwner(bool state)
    {
        isPartyOwner = state;
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool oldState, bool newState){
        if(!hasAuthority)
            return;

        AuthorityOnPartyOwnerStateUpdated?.Invoke(newState);
    }
    

}
