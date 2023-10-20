using DuloGames.UI;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Item : ScriptableObject
{
    public string Name;
    public int GoldCost;
    [Separator]
    public int HealthBonus;
    public int ManaBonus;

    public UIItemInfo ItemInfo;

    public virtual void OnAcquire(StatController statController)
    {
        statController.ChangeMaxStats(HealthBonus, ManaBonus);
    }
}
