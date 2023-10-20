using DuloGames.UI;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideWindows : NetworkBehaviour
{
    [SerializeField] private UIWindow shopWindow;
    [SerializeField] private ShopItemSlot shopSlotPrefab;
    [SerializeField] private Transform shopContentParent;


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
        }
        Debug.Log("asilxx121");

    }
}
