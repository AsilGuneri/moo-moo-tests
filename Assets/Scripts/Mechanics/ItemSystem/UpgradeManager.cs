using DuloGames.UI;
using Mirror;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class UpgradeManager : NetworkSingleton<UpgradeManager>
{
    [SerializeField] private UIWindow upgradeWindow;
    [SerializeField] private UpgradeSlot upgradeSlotPrefab;
    [SerializeField] private Transform shopContentParent;


    private void Start()
    {
        InitializeShop();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            upgradeWindow.Toggle();
        }
    }

    private void InitializeShop()
    {
        Extensions.DestroyAllChildren(shopContentParent);
        var itemList = AllItemsData.Instance.AllItems;
        foreach (var item in itemList) 
        {
            var slot = Instantiate(upgradeSlotPrefab, shopContentParent).GetComponent<UpgradeSlot>();
            slot.Setup(item);
        }
    }
}
