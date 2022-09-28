using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Hero Base Stats Data", fileName = "-HeroNameHere- Base Stats")]
public class HeroBaseStatsData : ScriptableObject
{
    public float AttackSpeed;
    public float Range;
    public int Hp;
    public int Damage;
}
