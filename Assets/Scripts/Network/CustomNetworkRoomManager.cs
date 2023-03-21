using Mirror;
using MyBox;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomNetworkRoomManager : NetworkRoomManager
{
    public List<CustomNetworkRoomPlayer> RoomPlayers = new List<CustomNetworkRoomPlayer>();
    public List<PlayerMertController> GamePlayers = new List<PlayerMertController>();

    public bool IsGameInProgress = false;

    [SerializeField] private bool useSteam = false;
    public bool UseSteam { get { return useSteam; } }


    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;



    private new void Start()
    {
        base.Start();
        if (!UseSteam) return;
        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            SceneManager.LoadScene(offlineScene);
            return;
        }
        else
        {
            StartHost();
            SteamMatchmaking.SetLobbyData(
                new CSteamID(callback.m_ulSteamIDLobby),
                "HostAddress",
                SteamUser.GetSteamID().ToString());
        }
    }
    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }
    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (NetworkServer.active) return;
        string hostAddress = SteamMatchmaking.GetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            "HostAddress");
        networkAddress = hostAddress;
        StartClient();
    }


    public void HostLobby()
    {
        if (useSteam)
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, maxConnections);
            return;
        }
        else
        {
            StartHost();
            return;
        }
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        int playerClassIndex = roomPlayer.GetComponent<CustomNetworkRoomPlayer>().RoomPlayerUI.CurrentClassIndex;
        var classData = PlayerSkillsDatabase.Instance.GetClassData(playerClassIndex);
        GameObject prefab = classData.ClassPrefab;
        GameObject gamePlayer = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        AddGamePlayer(gamePlayer.GetComponent<PlayerMertController>());
        return gamePlayer;
    }
    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
    {
        CustomNetworkRoomPlayer roomPlayer = (CustomNetworkRoomPlayer)Instantiate(roomPlayerPrefab);
        roomPlayer.SetPlayerData(conn.connectionId);
        return roomPlayer.gameObject;
    }
    private void AddGamePlayer(PlayerMertController newPlayer)
    {
        GamePlayers.Add(newPlayer);
        GoldManager.Instance.GameBank.AddBankAccount(newPlayer);
    }
}
