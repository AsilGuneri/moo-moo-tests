using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Steamworks;

public class CustomNetworkPlayer : NetworkBehaviour
{
    
    [SerializeField]private TextMeshProUGUI playerNameText;
    [SerializeField]private RawImage playerIcon;
    
    [SyncVar]public int connectionID;
    [SyncVar]public int playerIdNumber;
    [SyncVar]public ulong playerSteamID;

    [SyncVar(hook = nameof(PlayerNameUpdate))]
    public string playerName = "";

    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;
    protected Callback<AvatarImageLoaded_t> ImageLoaded;
    private bool avatarRecieved;

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
    
    private void Start() {
        ImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);
    }
    
    private void OnImageLoaded(AvatarImageLoaded_t callback){
        if(callback.m_steamID.m_SteamID != playerSteamID)
            return;

        if(!avatarRecieved)
            GetPlayerIcon();
    }
    
    public override void OnStartServer()
    {
        DontDestroyOnLoad(gameObject);
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        LobbyController.instance.FindLocalPlayer();
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        manager.players.Add(this);
        LobbyController.instance.UpdateLobbyName();
        LobbyController.instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        
        if(!isClientOnly)
            return;

        Debug.Log("On Stop Client Removed: " + playerName);
        ((CustomNetworkManager)NetworkManager.singleton).players.Remove(this);
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
        this.playerName = newValue;
        playerNameText.text = newValue;
        GetPlayerIcon();
        LobbyController.instance.UpdatePlayerList();
    }

    private void GetPlayerIcon(){
        if(avatarRecieved)
            return;

        Texture2D icon = Helper.GetTextureFromSteamID((CSteamID)playerSteamID);
        if(!icon)
            return;
            
        playerIcon.texture = icon;
        avatarRecieved = true;
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
