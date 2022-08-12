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

        Debug.Log("PallistitemCount: " + playerListItems.Count);
        Debug.Log("Manager Player Count: " + manager.players.Count);
        if(playerListItems.Count < manager.players.Count)
            CreateClientPlayerItem();

        else if(playerListItems.Count > manager.players.Count)
            RemovePlayerItem();

        else if(playerListItems.Count == manager.players.Count)
            UpdatePlayerItem();
        
    }

    public void FindLocalPlayer(){

    }

    public void CreateHostPlayerItem(){
        Debug.Log("Create Host Player Item");
        foreach(CustomNetworkPlayer player in manager.players){
            Debug.Log("Created!");
            InstantiatePlayerItem(player);
        }
        playerItemCreated = true;
    }

    public void CreateClientPlayerItem(){
        Debug.Log("Create Client Player Item");
        foreach(CustomNetworkPlayer player in manager.players){
            if(!playerListItems.Any(b => b.connectionID == player.connectionID)){
                Debug.Log("Client Created!");
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

        Debug.Log("Update Player Item");
        foreach(CustomNetworkPlayer player in manager.players){
            foreach(PlayerListItem listItem in playerListItems){
                if(listItem.connectionID == player.connectionID){
                    Debug.Log("Updated Player Item!");
                    listItem.playerName = player.playerName;
                    listItem.SetPlayerValues();
                }
            }
        }
    }

    public void RemovePlayerItem(){

        for (int j = 0; j < playerListItems.Count; j++)
        {
            bool isRemove = true;
            for (int i = 0; i < manager.players.Count; i++)
            {
                if(manager.players[i].connectionID == playerListItems[j].connectionID){
                    isRemove = false;
                    break;
                }
            }
            if(isRemove){
                GameObject objToRemove = playerListItems[j].gameObject;
                playerListItems.RemoveAt(j);
                Destroy(objToRemove);
                j--;
            }
        }
    
    }
}
