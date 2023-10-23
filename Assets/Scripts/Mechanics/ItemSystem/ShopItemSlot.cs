using DuloGames.UI;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlot : MonoBehaviour
{
    public Item Item { get => item; }

    [SerializeField] private UIItemSlot itemSlot;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button frameButton;

    private Item item;
    public void Setup(Item item)
    {
        this.item = item;
        var info = item.ItemInfo;
        itemSlot.Assign(info);
        nameText.text = info.Name;
        priceText.text = item.GoldCost.ToString();
        buyButton.onClick.AddListener(BuyItem);
        frameButton.onClick.AddListener(SelectItem);
    }
    private void BuyItem()
    {
        var customManager = NetworkRoomManager.singleton as CustomNetworkRoomManager;
        var player = customManager.GetLocalPlayer();
        if (LocalPlayerUI.Instance.InventoryUI.IsInventoryFull()) return;
        if (player.GoldController.GetGold() <= item.GoldCost) return;
        player.GoldController.CmdSpendGold(item.GoldCost);
        LocalPlayerUI.Instance.InventoryUI.AssignItem(item.ItemInfo);
        item.OnAcquire(player.StatController);
    }
    private void SelectItem()
    {
        ShowHideWindows.Instance.SelectItem(this);
    }
}
