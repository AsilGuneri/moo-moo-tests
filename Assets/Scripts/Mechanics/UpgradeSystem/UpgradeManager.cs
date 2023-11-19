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
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private Button selectButton;

    private UpgradeData selectedUpgrade;
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
        var upgrades = AllUpgradesData.Instance.GetUpgrades(ClassType.Archer, 1);
        foreach (var upgrade in upgrades)
        {
            var slot = Instantiate(upgradeSlotPrefab, upgradesContentParent).GetComponent<UpgradeSlot>();
            slot.Setup(upgrade);
        }
        ResetSelected();
    }
    public void OnSlotClicked(UpgradeSlot slot, bool isOn)
    {
        if(isOn)
        {
            this.selectedUpgrade = slot.Data;
            selectButton.interactable = true;
        }
        else if (!IsAnyToggleActive()) ResetSelected();
    }
    public void OnUpgradeAcquired()
    {
        if (selectedUpgrade == null) return;
        var manager = (CustomNetworkRoomManager) NetworkRoomManager.singleton;
        var statController = manager.GetLocalPlayer().StatController;
        
        selectedUpgrade.OnAcquire(statController);
        ResetSelected();
        upgradeWindow.Hide();
    }
    private void ResetSelected()
    {
        selectedUpgrade = null;
        selectButton.interactable = false;
    }
    bool IsAnyToggleActive()
    {
        foreach (var toggle in toggleGroup.ActiveToggles())
        {
            if (toggle.isOn)
            {
                return true;
            }
        }
        return false;
    }
}
