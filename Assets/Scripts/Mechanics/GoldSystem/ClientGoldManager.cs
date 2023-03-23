using Mirror;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utilities;

public class ClientGoldManager : NetworkSingleton<ClientGoldManager>
{
    [SerializeField] private TextMeshProUGUI goldText;

    int currentGold;

    public int GoldAmount 
    {
        get => currentGold; 
        private set => currentGold = value;
    }
    [TargetRpc]
    public void UpdateGold(NetworkConnection target, int goldAmount)
    {
        currentGold += goldAmount;
        UpdateGoldUI(goldAmount);
    }
    private void UpdateGoldUI(int goldAmount)
    {
        goldText.text = goldAmount.ToString();

    }

}
