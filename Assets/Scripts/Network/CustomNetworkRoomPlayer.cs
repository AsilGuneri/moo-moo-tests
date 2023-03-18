using Mirror;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomNetworkRoomPlayer : NetworkRoomPlayer
{
    [SyncVar] private int connectionId;
    /*[SyncVar]*/ public int CurrentMertIndex;

    public int ConnectionId { get { return connectionId; } private set { connectionId = value; } }

    private CustomNetworkRoomManager CustomManager;

    [SerializeField] private RectTransform playerUITransform;
    [SerializeField] private Button[] selectionButtons;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Image classImage;
    [SerializeField] private TextMeshProUGUI classNameText;
    [SerializeField] private Button readyButton;
    [SerializeField] private TextMeshProUGUI readyButtonText;


    private void Awake()
    {
        CustomManager = NetworkRoomManager.singleton as CustomNetworkRoomManager;
    }
    //for everyone
    private void OnEnable()
    {
        transform.localScale = Vector3.one;
        playerUITransform.SetParent(LobbyManager.Instance.RoomPlayerParent);
     //   SetCurrentIndex(0);
    }

    #region Network
    public override void OnStartClient()
    {
        base.OnStartClient();

        //For Everyone
        CustomManager.RoomPlayers.Add(this);
        DisableSelectionButtons();
        EnableSelectionButtons();
        readyButton.interactable = false;
        playerNameText.text = connectionId.ToString(); //temp
        CmdSetCurrentIndex(0);
        if (!hasAuthority) //For Everyone but owner
        {

        }
        else //For Owner
        {
            readyButton.interactable = true;
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
        int newIndex = isNext ? CurrentMertIndex + 1 : CurrentMertIndex - 1;
        int maxIndex = PlayerSkillsDatabase.Instance.ClassList.Count - 1;
        if (newIndex > maxIndex) newIndex = 0;
        if (newIndex < 0) newIndex = maxIndex;
        CmdSetCurrentIndex(newIndex);
    }
    public void ToggleReadyButton()
    {
        if (NetworkClient.active && isLocalPlayer)
        {

            if (readyToBegin)
            {
                CmdChangeReadyState(false);
                OnUnready();
            }
            else
            {
                CmdChangeReadyState(true);
                OnReady();

            }
        }
    }
    private void OnReady()
    {
        DisableSelectionButtons();
        readyButtonText.text = "Cancel";
    }
    private void OnUnready()
    {
        EnableSelectionButtons();
        readyButtonText.text = "Ready";
    }
    [Command(requiresAuthority = false)]
    private void CmdSetCurrentIndex(int index)
    {
        UpdatePlayerUI(index);
    }
    [ClientRpc]
    [ServerCallback]
    private void UpdatePlayerUI(int index)
    {
        CurrentMertIndex = index;
        var classData = PlayerSkillsDatabase.Instance.GetClassData(index);
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
}