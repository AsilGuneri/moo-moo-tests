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
    public int MoveSpeedBonus;
    [Range(0f, 1f)]
    public float CooldownBonus;
    [Separator]
    public int DamageBonus;
    [Range (0f, 1f)]
    public float AttackSpeedBonus;
    [Range(0f, 1f)]
    public float LifeStealBonus;
}
