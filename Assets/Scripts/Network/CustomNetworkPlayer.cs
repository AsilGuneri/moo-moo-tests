using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CustomNetworkPlayer : NetworkBehaviour
{


    [SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))]
    [SerializeField] private string displayName = "Missing Name";

    [SerializeField] private TMP_Text displayNameText;
    [SyncVar(hook = nameof(AuthorityHandlePartyOwnerStateUpdated))]private bool isPartyOwner = false;

    public static event Action ClientOnInfoUpdated;
    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;

    public bool GetIsPartyOwner(){
        return isPartyOwner;
    }
    
    public string GetDisplayName()
    {
        return displayName;
    }

    #region Server
    [Server] 
    public void SetDisplayName(string name)
    {
        displayName = name;
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


    #endregion
    #region Client
    private void HandleDisplayNameUpdated(string oldName, string newName)
    {
        displayNameText.text = newName;
    }

    [ContextMenu("Set name")]
    private void SetMyName()
    {
        CmdSetDisplayName("new name");
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
