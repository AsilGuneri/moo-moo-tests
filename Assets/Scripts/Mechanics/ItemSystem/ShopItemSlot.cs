using DuloGames.UI;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlot : MonoBehaviour
{
    [SerializeField] private UIItemSlot itemSlot;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;

    private Item item;
    public void Setup(Item item)
    {
        this.item = item;
        var info = item.ItemInfo;
        itemSlot.Assign(info);
        nameText.text = info.Name;
        priceText.text = item.GoldCost.ToString();
        buyButton.onClick.AddListener(BuyItem);
    }
    private void BuyItem()
    {
        var customManager = NetworkRoomManager.singleton as CustomNetworkRoomManager;
        var player = customManager.GetLocalPlayer();
        player.GoldController.CmdSpendGold(item.GoldCost);
    }
}
