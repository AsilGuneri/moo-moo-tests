using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Steamworks;

public class PlayerListItem : MonoBehaviour
{
    public string playerName;
    public int connectionID;
    public ulong playerSteamID;
    private bool avaterRecieved;

    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI playerReadyText;
    public RawImage playerIcon;

    public bool ready;

    protected Callback<AvatarImageLoaded_t> ImageLoaded;

    private void Start() {
        ImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);
    } 

    public void ChangeReadyStatus(){
        if(ready){
            playerReadyText.text = "Ready";
            playerReadyText.color = Color.green;
        }
        else{
            playerReadyText.text = "Not Ready";
            playerReadyText.color = Color.red;
        }
    }

    private void GetPlayerIcon(){
        int imageID = SteamFriends.GetLargeFriendAvatar((CSteamID)playerSteamID);

        if(imageID == -1)//If error
            return;

        playerIcon.texture = GetSteamImageAsTexture(imageID);
    }

    public void SetPlayerValues(){
        playerNameText.text = playerName;
        ChangeReadyStatus();
        if(!avaterRecieved)
            GetPlayerIcon();
    }

    private void OnImageLoaded(AvatarImageLoaded_t callback){
        //!ismine return
        if(callback.m_steamID.m_SteamID != playerSteamID)
            return;

        playerIcon.texture = GetSteamImageAsTexture(callback.m_iImage);
    }

    private Texture2D GetSteamImageAsTexture(int iImage){
        Texture2D texture = null;

        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);
        if(isValid){
            byte [] image = new byte[width * height * 4];

            isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));

            if(isValid){
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }

        avaterRecieved = true;
        return texture;
    }
}
