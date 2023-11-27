using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUIController : MonoBehaviour
{
    [SerializeField] private bool debug = false;
    [SerializeField] private Transform IconParent;
    [SerializeField] private Transform TimerParent;
    [SerializeField] private Transform Canvas;

    private Dictionary<string, ActiveStatus> activeEffects = new();
    private UnitController controller;

    private void Awake()
    {
        controller = GetComponent<UnitController>();
    }

    private void Start()
    {
        controller.Health.OnDeathServer += EndAll;
    }

    public void EndAll()
    {
        foreach (var activeEffect in new List<ActiveStatus>(activeEffects.Values))
        {
            EndStatus(activeEffect.Effect.effectName);
        }
    }

    public void EndStatus(string name)
    {
        if (activeEffects.ContainsKey(name))
        {
            var activeEffect = activeEffects[name];
            Destroy(activeEffect.IconObj.gameObject);
            Destroy(activeEffect.Timer.gameObject);
            activeEffects.Remove(name);
        }
        else if (debug) Debug.Log("Disactive status tried to end : " + name + " userName : " + name);
    }

    public void StartStatus(string name, float time = 0)
    {
        var status = StatusEffectManager.Instance.GetStatusEffect(name);
        StartCoroutine(StartStatus(status, time));
    }

    private IEnumerator StartStatus(StatusEffect status, float time = 0)
    {
        if (!activeEffects.ContainsKey(status.effectName))
        {
            //controller.Health.OpenHealthBar();
            ActiveStatus activeStatus = new ActiveStatus
            {
                Effect = status,
                IconObj = InstantiateIcon(status),
                Timer = InstantiateTimer(time, status)
            };

            activeEffects.Add(status.effectName, activeStatus);

            yield return Extensions.GetWait(time);

            EndStatus(status.effectName);
        }
        else if (debug) Debug.Log("Active status tried to start : " + status.effectName + " userName : " + name);
    }

    private GameObject InstantiateIcon(StatusEffect status)
    {
        var icon = Instantiate(StatusEffectManager.Instance.IconPrefab, IconParent).GetComponent<StatusIcon>();
        icon.ChangeIconSprite(status.iconSprite);
        return icon.gameObject;
    }

    private StatusTimer InstantiateTimer(float duration, StatusEffect status)
    {
        var timer = Instantiate(StatusEffectManager.Instance.TimerPrefab, TimerParent).GetComponent<StatusTimer>();
        timer.StartTimer(duration, status.timerColor);
        return timer;
    }

    public void SetCanvasHeight(float height)
    {
        Canvas.transform.localPosition = new Vector3(0, height, 0);
    }
}
