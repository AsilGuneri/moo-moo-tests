using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "StatusEffectManager", menuName = "Scriptable Objects/Managers/StatusEffectManager")]
public class StatusEffectManager : ScriptableSingleton<StatusEffectManager>
{
    public GameObject IconPrefab;
    public GameObject TimerPrefab;

    [SerializeField] private List<StatusEffect> effects = new List<StatusEffect>();

    public StatusEffect GetStatusEffect(string name)
    {
        foreach (var effect in effects)
        {
            if (effect.effectName == name)
            {
                return effect;
            }
        }
        return null;
    }
}
[Serializable]
public class StatusEffect
{
    public string effectName;
    public Sprite iconSprite;
    public Color timerColor;

}
public class ActiveStatus
{
    public StatusEffect Effect { get; set; }
    public GameObject IconObj { get; set; }
    public StatusTimer Timer { get; set; }
    public Coroutine Coroutine { get; set; }
}
