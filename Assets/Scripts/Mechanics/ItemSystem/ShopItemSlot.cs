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

    private Item item;
    public void Setup(Item item)
    {
        this.item = item;
        var info = item.ItemInfo;
        itemSlot.Assign(info);
        nameText.text = info.Name;
    }


}
