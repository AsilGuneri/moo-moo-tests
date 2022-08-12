using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Steamworks;

public class CustomNetworkPlayer : NetworkBehaviour
{



    [SerializeField] private TMP_Text playerNameText;
    [SyncVar(hook = nameof(AuthorityHandlePartyOwnerStateUpdated))]private bool isPartyOwner = false;

    public static event Action ClientOnInfoUpdated;
    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;


    [SyncVar] public int connectionID;
    [SyncVar] public int playerIdNumber;
    [SyncVar] public ulong playerSteamID;
    [SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))]
    public string playerName = "Missing Name";

    
    private CustomNetworkManager _manager;
    private CustomNetworkManager manager{
        get{
            if(_manager != null)
                return _manager;
            return _manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }


    public bool GetIsPartyOwner(){
        return isPartyOwner;
    }
    
    public string GetDisplayName()
    {
        return playerName;
    }

    #region Server
    [Server] 
    public void SetDisplayName(string name)
    {
        playerName = name;
    }

    [Server] 
    public void SetPartyOwner(bool state)
    {
        isPartyOwner = state;
    }

    [Command]
    public void CmdStartGame(){
        if(!isPartyOwner)
            return;

        ((CustomNetworkManager)NetworkManager.singleton).StartGame();

    }
    
    [Command] //Clients calling a method on server
    private void CmdSetDisplayName(string newDisplayName) 
    {
        if (newDisplayName.Length < 2 || newDisplayName.Length > 20) { return; } //Server authorization
        RpcLogNewName(newDisplayName);
        SetDisplayName(newDisplayName);
    }

    public override void OnStartServer()
    {
        DontDestroyOnLoad(gameObject);
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(SteamFriends.GetPersonaName().ToString());
        LobbyController.instance.UpdateLobbyName();
    }


    #endregion
    #region Client
    private void HandleDisplayNameUpdated(string oldName, string newName)
    {
        playerNameText.text = newName;
    }

    [ClientRpc] //Server calling a method on all clients
    private void RpcLogNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }

    public override void OnStartClient()
    {
        
        if(NetworkServer.active)
            return;
            
        DontDestroyOnLoad(gameObject);
        ((CustomNetworkManager)NetworkManager.singleton).players.Add(this);
        LobbyController.instance.UpdateLobbyName();
        LobbyController.instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        // UnitManager.Instance.UnregisterUnits(new NetworkIdentityReference(gameObject.GetComponent<NetworkIdentity>()), UnitType.Player);
        
        ClientOnInfoUpdated?.Invoke();

        if(!isClientOnly)
            return;

        ((CustomNetworkManager)NetworkManager.singleton).players.Remove(this);
    }

    #endregion

    private void AuthorityHandlePartyOwnerStateUpdated(bool oldState, bool newState){
        if(!hasAuthority)
            return;

        AuthorityOnPartyOwnerStateUpdated?.Invoke(newState);
    }
    
    private void ClientHandleDisplayNameUpdated(string oldDisplayName, string newDisplayName)
    {
        ClientOnInfoUpdated?.Invoke();
    }

}
