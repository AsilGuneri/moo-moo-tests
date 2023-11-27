using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Hero Base Stats Data", fileName = "-HeroNameHere- Base Stats")]
public class HeroBaseStatsData : ScriptableObject
{
    public float AttackSpeed;
    public float AttackRange;
    public int Health;
    public int Mana;
    public int Damage;
    [Range(0f, 1f)]
    public float AdditionalDamageRatio;
    public float MoveSpeed;
}
