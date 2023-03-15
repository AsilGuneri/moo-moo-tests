using Mirror;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomNetworkRoomPlayer : NetworkRoomPlayer
{
    [SyncVar]private int connectionId;
    //[SyncVar(hook = nameof(UpdatePlayerName))] private string playerName;

    public int ConnectionId { get { return connectionId; } private set { connectionId = value; } }

    private CustomNetworkRoomManager CustomManager;
    private int currentOfflineIndex = 0;

    [SerializeField] private Button[] selectionButtons;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Image classImage;
    [SerializeField] private TextMeshProUGUI classNameText;

    private void Awake()
    {
        CustomManager = NetworkRoomManager.singleton as CustomNetworkRoomManager;
    }
    //for everyone
    private void OnEnable()
    {
        transform.localScale = Vector3.one;
        transform.SetParent(LobbyManager.Instance.RoomPlayerParent);
    }

    #region Network
    public override void OnStartClient()
    {
        base.OnStartClient();

        //For Everyone
        CustomManager.RoomPlayers.Add(this);
        DisableSelectionButtons();
        EnableSelectionButtons();
        playerNameText.text = connectionId.ToString(); //temp
        ChangeCharacter(0);

        if (!hasAuthority) //For Everyone but owner
        {

        }
        else //For Owner
        {
            //name variable should be syncvar and set here.
        }
    }
    public override void OnStopClient()
    {
        base.OnStopClient();
        CustomManager.RoomPlayers.Remove(this);

    }
    #endregion
    public void SetPlayerData(int connectionId)
    {
        ConnectionId = connectionId;
        //playerName = ConnectionId.ToString();
    }
    //Has a reference on Next/Previous buttons
    public void ChangeCharacterLinear(bool isNext)
    {
        int newIndex = isNext ? currentOfflineIndex + 1 : currentOfflineIndex - 1;
        int maxIndex = PlayerSkillsDatabase.Instance.ClassList.Count - 1;
        if(newIndex > maxIndex) newIndex = 0;
        if(newIndex < 0) newIndex = maxIndex;
        ChangeCharacter(newIndex);
    }
    private void ChangeCharacter(int index)
    {
        var classData = PlayerSkillsDatabase.Instance.GetClassData(index);
        currentOfflineIndex = index;
        classImage.sprite = classData.ClassLobbySprite;
        classNameText.text = classData.Class.ToString();
        //how to sync these on ready
    }

    private void DisableSelectionButtons()
    {
        foreach (var button in selectionButtons)
        {
            if (button.interactable)
            {
                button.interactable = false;
            }
        }
    }

    private void EnableSelectionButtons()
    {
        if (hasAuthority)
        {
            foreach (var button in selectionButtons)
            {
                button.interactable = true;
            }
        }

    }

    private void UpdatePlayerName(string oldValue, string newValue)
    {
        playerNameText.text = newValue;
    }
}