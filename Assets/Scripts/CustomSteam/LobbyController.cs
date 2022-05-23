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
    public GameObject playerListViewContainer;
    public GameObject playerListItemPrefab;
    public GameObject localPlayerObject;

    //Other Data
    public ulong currentLobbyID;
    public bool playerItemCreated;
    private List<PlayerListItem> playerListItems = new List<PlayerListItem>();
    public PlayerObjectController localPlayerController;

    //Ready
    public Button startGameButton;
    public TextMeshProUGUI readyButtonText;

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

    public void ReadyPlayer(){
        localPlayerController.ChangeReady();
    }

    public void UpdateButton(){
        if(localPlayerController.ready){
            readyButtonText.text ="Not Ready";
        }
        else{
            readyButtonText.text ="Ready";
        }
    }

    public void CheckIfAllReady(){
        bool allReady = false;

        foreach(PlayerObjectController player in _manager.gamePlayers){
            if(player.ready){
                allReady = true;
            }
            else{
                allReady = false;
                break;
            }
        }
        if(allReady){
            startGameButton.interactable = localPlayerController.playerIDNumber == 1;
        }
        else{
            startGameButton.interactable = false;
        }
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
        foreach(PlayerObjectController player in _manager.gamePlayers){
            GameObject newPlayerItem = Instantiate(playerListItemPrefab) as GameObject;
            PlayerListItem newPlayerItemScript = newPlayerItem.GetComponent<PlayerListItem>();

            newPlayerItemScript.playerName = player.playerName;
            newPlayerItemScript.connectionID = player.connectionID;
            newPlayerItemScript.playerSteamID = player.playerSteamID;
            newPlayerItemScript.ready = player.ready;
            newPlayerItemScript.SetPlayerValues();

            newPlayerItem.transform.SetParent(playerListViewContainer.transform);
            newPlayerItem.transform.localScale = Vector3.one;

            playerListItems.Add(newPlayerItemScript);
        }
        playerItemCreated = true;
    }

    public void CreateClientPlayerItem(){
        foreach(PlayerObjectController player in _manager.gamePlayers){
            if(!playerListItems.Any(b => b.connectionID == player.connectionID)){
                MenuManager.instance.OpenMenu("Room");
                GameObject newPlayerItem = Instantiate(playerListItemPrefab) as GameObject;
                PlayerListItem newPlayerItemScript = newPlayerItem.GetComponent<PlayerListItem>();

                newPlayerItemScript.playerName = player.playerName;
                newPlayerItemScript.connectionID = player.connectionID;
                newPlayerItemScript.playerSteamID = player.playerSteamID;
                newPlayerItemScript.ready = player.ready;
                newPlayerItemScript.SetPlayerValues();

                newPlayerItem.transform.SetParent(playerListViewContainer.transform);
                newPlayerItem.transform.localScale = Vector3.one;

                playerListItems.Add(newPlayerItemScript);
            }
        }
    }

    public void UpdatePlayerItem(){
        
        foreach(PlayerObjectController player in _manager.gamePlayers){
            foreach(PlayerListItem playerListItemScript in playerListItems){
                if(playerListItemScript.connectionID == player.connectionID){
                    playerListItemScript.playerName = player.playerName;
                    playerListItemScript.ready = player.ready;
                    playerListItemScript.SetPlayerValues();
                    if(player == localPlayerController){
                        UpdateButton();
                    }
                }
            }
        }
        CheckIfAllReady();
    }

    public void RemovePlayerItem(){
        List<PlayerListItem> playerListItemsToRemove = new List<PlayerListItem>();

        foreach(PlayerListItem playerListItem in playerListItems){
            if(!_manager.gamePlayers.Any(b => b.connectionID == playerListItem.connectionID)){
                playerListItemsToRemove.Add(playerListItem);
            }
        }
        if(playerListItemsToRemove.Count > 0){
            foreach(PlayerListItem playerListItemToRemove in playerListItemsToRemove){
                if(!playerListItemToRemove)
                    continue;
                GameObject objectToRemove = playerListItemToRemove.gameObject;
                playerListItems.Remove(playerListItemToRemove);
                Destroy(objectToRemove);
                objectToRemove = null;

            }
        }
    }

    public void FindLocalPlayer(){
        localPlayerObject = GameObject.Find("LocalGamePlayer");
        localPlayerController = localPlayerObject.GetComponent<PlayerObjectController>();
    }

    public void StartGame(string sceneName){
        localPlayerController.CanStartGame(sceneName);
    }

}
