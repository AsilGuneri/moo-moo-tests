using Mirror;
using MyBox;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomNetworkRoomManager : NetworkRoomManager
{
    public List<CustomNetworkRoomPlayer> RoomPlayers = new List<CustomNetworkRoomPlayer>();
    public List<UnitController> GamePlayers = new List<UnitController>();

    public bool IsGameInProgress = false;

    [SerializeField] private bool useSteam = false;
    public bool UseSteam { get { return useSteam; } }


    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;

    [Header("Loading Screen")]
    private LoadingManager loader;

    private new void Start()
    {
        base.Start();
        loader = GetComponent<LoadingManager>();
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
        }
        else
        {
            StartHost();
            loader.Load(loadingSceneAsync);
        }
    }

    public override void OnRoomServerPlayersReady()
    {
        base.OnRoomServerPlayersReady();
        loader.Load(loadingSceneAsync);
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        int playerClassIndex = roomPlayer.GetComponent<CustomNetworkRoomPlayer>().RoomPlayerUI.CurrentClassIndex;
        var classData = PlayerSkillsDatabase.Instance.GetClassData(playerClassIndex);
        GameObject prefab = classData.ClassPrefab;
        GameObject gamePlayer = Instantiate(prefab, Vector3.zero, Quaternion.identity);

        var playerController = gamePlayer.GetComponent<UnitController>();
        //playerController.PlayerName = conn.connectionId.ToString(); // temp
        GamePlayers.Add(playerController);
        return gamePlayer;
    }
    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
    {
        CustomNetworkRoomPlayer roomPlayer = (CustomNetworkRoomPlayer)Instantiate(roomPlayerPrefab);
        roomPlayer.SetPlayerData(conn.connectionId);
        return roomPlayer.gameObject;
    }
    //public override void OnServerSceneChanged(string sceneName)
    //{
    //    base.OnServerSceneChanged(sceneName);
    //    if (sceneName == GameplayScene)
    //    {
    //        ObjectPooler.Instance.Initialize();
    //    }
    //}
    //public override void OnClientSceneChanged()
    //{
    //    base.OnClientSceneChanged();
    //    Debug.Log("asilxx name : " + SceneManager.GetActiveScene().name);
    //    if (SceneManager.GetActiveScene().name == "GameScene")
    //    {
    //        ObjectPooler.Instance.Initialize();
    //    }
    //}
}
