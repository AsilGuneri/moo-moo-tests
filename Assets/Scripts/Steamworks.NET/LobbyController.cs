using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Steamworks;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class LobbyController : MonoBehaviour
{
    public static LobbyController instance;

    public TextMeshProUGUI lobbyNameText;

    public GameObject playerListItemPrefab;
    [SerializeField]private Transform playerListContainer;

    public ulong currentLobbyID;
    public bool playerItemCreated = false;
    private List<PlayerListItem> playerListItems = new List<PlayerListItem>();

    private CustomNetworkManager _manager;

    private CustomNetworkManager manager{
        get{
            if(_manager != null)
                return _manager;
            return _manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    private void Awake() {
        instance = this;
    }

    public void UpdateLobbyName(){
        currentLobbyID = SteamLobby.instance.currentLobbyID;
        lobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(currentLobbyID), "name");
    }

    public void UpdatePlayerList(){
        if(!playerItemCreated)
            CreateHostPlayerItem();

        if(playerListItems.Count < manager.players.Count)
            CreateClientPlayerItem();

        if(playerListItems.Count > manager.players.Count)
            RemovePlayerItem();

        if(playerListItems.Count == manager.players.Count)
            UpdatePlayerItem();
        
    }

    public void FindLocalPlayer(){

    }

    public void CreateHostPlayerItem(){
        foreach(CustomNetworkPlayer player in manager.players){
            InstantiatePlayerItem(player);
        }
        playerItemCreated = true;
    }

    public void CreateClientPlayerItem(){

        foreach(CustomNetworkPlayer player in manager.players){
            if(!playerListItems.Any(b => b.connectionID == player.connectionID)){
                InstantiatePlayerItem(player);
            }
        }
    }

    public void InstantiatePlayerItem(CustomNetworkPlayer player){

        GameObject newPlayerItem = Instantiate(playerListItemPrefab) as GameObject;
        PlayerListItem newPlayerListItem = newPlayerItem.GetComponent<PlayerListItem>();

        newPlayerListItem.playerName = player.playerName;
        newPlayerListItem.connectionID = player.connectionID;
        newPlayerListItem.playerSteamID = player.playerSteamID;

        newPlayerItem.transform.SetParent(playerListContainer);

        playerListItems.Add(newPlayerListItem);
    }

    public void UpdatePlayerItem(){

        foreach(CustomNetworkPlayer player in manager.players){
            foreach(PlayerListItem listItem in playerListItems){
                if(listItem.connectionID == player.connectionID){
                    listItem.playerName = player.playerName;
                    listItem.SetPlayerValues();
                }
            }
        }
    }

    public void RemovePlayerItem(){
        List<PlayerListItem> playerListItemsToRemove = new List<PlayerListItem>();

        foreach(PlayerListItem playerListItem in playerListItems){
            if(!manager.players.Any(b => b.connectionID == playerListItem.connectionID)){
                playerListItemsToRemove.Add(playerListItem);
            }
        }
        if(playerListItemsToRemove.Count > 0){
            foreach(PlayerListItem listItemToRemove in playerListItemsToRemove){
                GameObject objectToRemove = listItemToRemove.gameObject;
                playerListItemsToRemove.Remove(listItemToRemove);
                Destroy(objectToRemove);
                objectToRemove = null;

            }
        }
    }
}
