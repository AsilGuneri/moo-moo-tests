using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using Steamworks;
using System.Linq;

public class LobbyController : MonoBehaviour
{
    public static LobbyController instance;

    public TextMeshProUGUI lobbyNameText;

    //Player Data
    public GameObject playerListViewContent;
    public GameObject playerListItemPrefab;
    public GameObject localPlayerObject;

    //Other Data
    public ulong currentLobbyID;
    public bool playerItemCreated;
    private List<PlayerListItem> playerListItems = new List<PlayerListItem>();
    public PlayerObjectController localPlayerController;

    private CustomNetworkManager manager;
    private CustomNetworkManager _manager{
        get{
            if(manager != null){
                return manager;
            }
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    private void Awake() {
        if(instance == null)
            instance = this;
    }

    public void UpdateLobbyName(){
        currentLobbyID = _manager.GetComponent<SteamLobby>().currentLobbyID;
        lobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(currentLobbyID), "name");
    }

    public void UpdatePlayerList(){
        if(!playerItemCreated){
            CreateHostPlayerItem();
        }

        if(playerListItems.Count < _manager.gamePlayers.Count){
            CreateClientPlayerItem();
        }
        else if(playerListItems.Count > _manager.gamePlayers.Count){
            RemovePlayerItem();
        }
        else{
            UpdatePlayerItem();
        }
    }

    public void CreateHostPlayerItem(){
        
    }

    public void CreateClientPlayerItem(){

    }

    public void UpdatePlayerItem(){
        
    }

    public void RemovePlayerItem(){

    }

    public void FindLocalPlayer(){
        localPlayerObject = GameObject.Find("LocalGamePlayer");
        localPlayerController = localPlayerObject.GetComponent<PlayerObjectController>();
    }

}
