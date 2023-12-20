using DuloGames.UI;
using Mirror;
using MyBox;
using System;
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

    public List<UpgradeDataLevelPair> acquiredUpgrades = new();
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
        var randomUpgrades = AllUpgradesData.Instance.GetRandomUpgrades(ClassType.Archer);

        foreach (var upgrade in randomUpgrades)
        {
            var slot = Instantiate(upgradeSlotPrefab, upgradesContentParent).GetComponent<UpgradeSlot>();
            slot.Setup(upgrade.data, upgrade.level);
        }
    }
    public void OnUpgradeAcquired(UpgradeData data, int level)
    {
        // if (selectedUpgrade == null) return;
        //var manager = (CustomNetworkRoomManager) NetworkRoomManager.singleton;
        //var statController = manager.GetLocalPlayer().StatController;
        //if (acquiredUpgrades.ContainsKey(data)) acquiredUpgrades[data] = level;
        //else acquiredUpgrades.Add(data, level);
        acquiredUpgrades.Add(new UpgradeDataLevelPair(data, level));
        //selectedUpgrade.OnAcquire(statController);
        upgradeWindow.Hide();
    }
}