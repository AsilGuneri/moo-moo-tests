using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Steamworks;

public static class Helper
{
    public static Texture2D GetTextureFromSteamID(CSteamID steamID){
        
        int imageID = SteamFriends.GetLargeFriendAvatar(steamID);
        if(imageID == -1)
            return null;
        Texture2D texture = null;

        bool isValid = SteamUtils.GetImageSize(imageID, out uint width, out uint height);
        if(isValid){
            byte [] image = new byte[width * height * 4];

            isValid = SteamUtils.GetImageRGBA(imageID, image, (int)(width * height * 4));

            if(isValid){
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }

        return texture;
    }

}
