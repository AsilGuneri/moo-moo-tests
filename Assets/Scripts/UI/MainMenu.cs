using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using Steamworks;

public class MainMenu : MonoBehaviour
{
    //Steam
    [SerializeField] private bool useSteam = false;
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;


    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private GameObject joinLobbyPanel = null;
    [SerializeField] private GameObject lobbyPanel = null;
    [SerializeField] private TMP_InputField addressInput = null;
    [SerializeField] private Button joinButton = null;
    [SerializeField] private Button startGameButton = null;
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];

    private void Start() {
        // CustomNetworkPlayer.AuthorityOnPartyOwnerStateUpdated += AuthorityHandlePartyOwnerStateUpdated;
        // CustomNetworkPlayer.ClientOnInfoUpdated += ClientHandleInfoUpdated;
        
        if (!useSteam) { return; }

        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

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
        // CustomNetworkManager.ClientOnConnected -= HandleClientConnected;
        // CustomNetworkManager.ClientOnDisonnected -= HandleClientDisconnected;
        // CustomNetworkPlayer.AuthorityOnPartyOwnerStateUpdated -= AuthorityHandlePartyOwnerStateUpdated;
        // CustomNetworkPlayer.ClientOnInfoUpdated -= ClientHandleInfoUpdated;

    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool state){
        startGameButton.gameObject.SetActive(state);
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
        }
        Back();
    }
    
    private void ClientHandleInfoUpdated()
    {
        List<CustomNetworkPlayer> players = ((CustomNetworkManager)NetworkManager.singleton).players;

        // for (int i = 0; i < players.Count; i++)
        // {
        //     playerNameTexts[i].text = players[i].GetDisplayName();
        // }

        // for (int i = players.Count; i < playerNameTexts.Length; i++)
        // {
        //     playerNameTexts[i].text = "Waiting For Player...";
        // }

        startGameButton.interactable = players.Count >= 1;
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


    //Steam
    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (NetworkServer.active) { return; }

        landingPagePanel.SetActive(false);
    }
}
