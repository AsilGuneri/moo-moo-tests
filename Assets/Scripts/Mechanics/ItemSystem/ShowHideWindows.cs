using DuloGames.UI;
using Mirror;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class ShowHideWindows : NetworkSingleton<ShowHideWindows>
{
    [SerializeField] private UIWindow shopWindow;
    [SerializeField] private ShopItemSlot shopSlotPrefab;
    [SerializeField] private Transform shopContentParent;
    [Separator("Selected Item")]
    [SerializeField] private TextMeshProUGUI selectedItemName;
    [SerializeField] private Image selectedItemIcon;
    [SerializeField] private TextMeshProUGUI selectedItemDescripton;
    [SerializeField] private TextMeshProUGUI selectedItemStats;


    private List<ShopItemSlot> shopItems = new();
    private ShopItemSlot selectedItem;

    private void Start()
    {
        InitializeShop();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            shopWindow.Toggle();
        }
    }

    private void InitializeShop()
    {
        Extensions.DestroyAllChildren(shopContentParent);
        var itemList = AllItemsData.Instance.AllItems;
        foreach (var item in itemList) 
        {
            var slot = Instantiate(shopSlotPrefab, shopContentParent).GetComponent<ShopItemSlot>();
            slot.Setup(item);
            shopItems.Add(slot);
        }
    }
    public void SelectItem(ShopItemSlot slot)
    {
        if (slot == selectedItem) return;
        selectedItem = slot;
        selectedItemName.text = slot.Item.ItemInfo.Name;
        selectedItemIcon.sprite = slot.Item.ItemInfo.Icon;
        selectedItemDescripton.text = slot.Item.ItemInfo.Description;
    //    selectedItemStats.text 
    }
}
