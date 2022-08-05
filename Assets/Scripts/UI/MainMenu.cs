using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private GameObject joinLobbyPanel = null;
    [SerializeField] private GameObject lobbyPanel = null;
    [SerializeField] private TMP_InputField addressInput = null;
    [SerializeField] private Button joinButton = null;
    [SerializeField] private Button startGameButton = null;

    private void Start() {
        CustomNetworkPlayer.AuthorityOnPartyOwnerStateUpdated += AuthorityHandlePartyOwnerStateUpdated;
    }

    private void OnEnable() {
        CustomNetworkManager.ClientOnConnected += HandleClientConnected;
        CustomNetworkManager.ClientOnDisonnected += HandleClientDisconnected;
    }

    private void OnDisable() {
        CustomNetworkManager.ClientOnConnected -= HandleClientConnected;
        CustomNetworkManager.ClientOnDisonnected -= HandleClientDisconnected;
    }

    private void OnDestroy() {
        CustomNetworkManager.ClientOnConnected -= HandleClientConnected;
        CustomNetworkManager.ClientOnDisonnected -= HandleClientDisconnected;
        CustomNetworkPlayer.AuthorityOnPartyOwnerStateUpdated -= AuthorityHandlePartyOwnerStateUpdated;
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool state){
        startGameButton.gameObject.SetActive(state);
    }

    public void HostLobby(){
        landingPagePanel.SetActive(false);
        lobbyPanel.SetActive(true);
        NetworkManager.singleton.StartHost();
    }

    public void JoinLobbyPanel(){
        landingPagePanel.SetActive(false);
        joinLobbyPanel.SetActive(true);
    }

    public void Back(){
        joinLobbyPanel.SetActive(false);
        lobbyPanel.SetActive(false);
        landingPagePanel.SetActive(true);
    }

    public void Join(){
        string address = addressInput.text;

        joinButton.interactable = false;
        NetworkManager.singleton.networkAddress = address;
        NetworkManager.singleton.StartClient();
    }

    public void StartGame(){
        NetworkClient.connection.identity.GetComponent<CustomNetworkPlayer>().CmdStartGame();
    }

    public void LeaveLobby(){
        if(NetworkServer.active && NetworkClient.isConnected){
            NetworkManager.singleton.StopHost();
        }
        else{
            NetworkManager.singleton.StopClient();
            Back();
        }
    }

    private void HandleClientConnected(){
        joinButton.interactable = true;
        joinLobbyPanel.SetActive(false);
        landingPagePanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    private void HandleClientDisconnected(){
        joinButton.interactable = true;
    }
}
