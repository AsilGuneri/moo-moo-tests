using Mirror;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayerUI : NetworkBehaviour
{
    public int CurrentClassIndex;

    [Separator("Ready Button References")]
    [SerializeField] private Button readyButton;
    [SerializeField] private Image readyButtonImage;
    [SerializeField] private TextMeshProUGUI readyButtonText;

    [Separator("Selection Buttons References")]
    [SerializeField] private Button[] selectionButtons;

    [Separator("Player Name")]
    [SerializeField] private TextMeshProUGUI playerNameText;

    [Separator("Class UI")]
    [SerializeField] private Image classImage;
    [SerializeField] private TextMeshProUGUI classNameText;

    private int connectionId;
   // private new bool hasAuthority;

    #region General
    public void InitializeUI(bool hasAuthority, int connectionId)
    {
        //this.hasAuthority = hasAuthority;
        this.connectionId = connectionId;
        InitializeSelectionButtons();
        InitializeReadyButton();
        playerNameText.text = connectionId.ToString();
    }
    public void ChangeCharacterLinear(bool isNext)
    {
        int newIndex = isNext ? CurrentClassIndex + 1 : CurrentClassIndex - 1;
        int maxIndex = PlayerSkillsDatabase.Instance.ClassList.Count - 1;
        if (newIndex > maxIndex) newIndex = 0;
        if (newIndex < 0) newIndex = maxIndex;
        CmdSetCurrentIndex(newIndex);
    }
    #endregion

    #region Ready Button
    private void InitializeReadyButton()
    {
        if(!hasAuthority)//default way of the prefab for the player with authority
        {
            readyButton.interactable = false;
            readyButtonText.text = "Not Ready";
            readyButtonImage.enabled = false;
        }  
    }

    public void OnSetReady(bool setToReady)
    {
        if (setToReady)
        {
            DisableSelectionButtons();
            if (hasAuthority)
            {
                readyButtonText.text = "Cancel";
            }
            else
            {
                readyButtonText.text = "Not Ready";
            }
        }
        else
        {
            EnableSelectionButtons();
            readyButtonText.text = "Ready";
        }

    }
    #endregion

    #region SelectionButtons
    public void EnableSelectionButtons()
    {
        if (hasAuthority)
        {
            foreach (var button in selectionButtons)
            {
                button.interactable = true;
            }
        }
    }
    public void DisableSelectionButtons()
    {
        foreach (var button in selectionButtons)
        {
            button.interactable = false;
        }
    }
    private void InitializeSelectionButtons()
    {
        if (!hasAuthority)
        {
            DisableSelectionButtons();
        }
    }
    #endregion

    #region PlayerUI

    [Command(requiresAuthority = true)]
    public void CmdSetCurrentIndex(int index)
    {
        CurrentClassIndex = index;
        UpdatePlayerUI(index);
    }

    [Command(requiresAuthority = false)]
    public void CmdRefreshPlayerUI()
    {
        RefreshPlayerUI(CurrentClassIndex); //index on the host
    }

    [ClientRpc]
    [ServerCallback]
    private void UpdatePlayerUI(int index)
    {
        UpdateUI(index);
    }
    [ClientRpc]
    [ServerCallback]
    private void RefreshPlayerUI(int indexOnServer)
    {
        CurrentClassIndex = indexOnServer;
        UpdateUI(CurrentClassIndex);
        //how to sync these on ready
    }
    private void UpdateUI(int index)
    {
        var classData = PlayerSkillsDatabase.Instance.GetClassData(index);
        classImage.sprite = classData.ClassLobbySprite;
        classNameText.text = classData.Class.ToString();
    }
    #endregion
}
