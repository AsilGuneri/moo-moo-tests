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
    [SerializeField] private Transform upgradesContentParent;

    private void Start()
    {
        InitializeUpgrades();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            upgradeWindow.Toggle();
            if (upgradeWindow.IsOpen) InitializeUpgrades();
        }
    }

    private void InitializeUpgrades()
    {
        Extensions.DestroyAllChildren(upgradesContentParent);
        //var upgrades = AllUpgradesData.Instance.GetUpgrades(ClassType.Archer, 2);
        //for (int i = 0; i < 4; i++)
        //{
        //    var upgrade = AllUpgradesData.Instance.GetRandomUpgrade(ClassType.Archer);
        //    var slot = Instantiate(upgradeSlotPrefab, upgradesContentParent).GetComponent<UpgradeSlot>();
        //    slot.Setup(upgrade);
        //}

    }
  
    public void OnUpgradeAcquired()
    {
       // if (selectedUpgrade == null) return;
        //var manager = (CustomNetworkRoomManager) NetworkRoomManager.singleton;
        //var statController = manager.GetLocalPlayer().StatController;
        
        //selectedUpgrade.OnAcquire(statController);
        upgradeWindow.Hide();
    }
 
}
