using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using Steamworks;

public class PlayerListItem : MonoBehaviour
{
    public string playerName;
    public int connectionID;
    public ulong playerSteamID;
    private bool avatarRecieved;

    public TextMeshProUGUI playerNameText;
    public RawImage playerIcon;

    protected Callback<AvatarImageLoaded_t> ImageLoaded;

    private void Start() {
        ImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);
    }

    private void OnImageLoaded(AvatarImageLoaded_t callback){
        if(callback.m_steamID.m_SteamID != playerSteamID)
            return;

        if(!avatarRecieved)
            GetPlayerIcon();
    }

    private void GetPlayerIcon(){
        Texture2D icon = Helper.GetTextureFromSteamID((CSteamID)playerSteamID);
        if(!icon)
            return;
        playerIcon.texture = icon;
        avatarRecieved = true;
    }

    public void SetPlayerValues(){
        playerNameText.text = playerName;
        if(!avatarRecieved)
            GetPlayerIcon();
    }

}
