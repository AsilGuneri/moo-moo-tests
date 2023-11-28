using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUIController : MonoBehaviour
{
    [SerializeField] private Transform iconParent;
    [SerializeField] private Transform barParent;
    [SerializeField] private Transform Canvas;

    Dictionary<Status,StatusUI> statusUIPairs = new Dictionary<Status,StatusUI>();

    private void Update()
    {
        foreach (KeyValuePair<Status, StatusUI> pair in statusUIPairs)
        {
            StatusUI statusUI = pair.Value;
            statusUI.OnUpdate(pair.Key.Time);
        }
    }

    public void OnStatusStart(Status status)
    {
        StatusUI statusUI = Instantiate(StatusManager.Instance.StatusUIPrefab, iconParent).GetComponent<StatusUI>();
        statusUI.Setup(status.Data, status.Time, iconParent, barParent);
        statusUIPairs.Add(status, statusUI);
    }
    public void OnStatusEnd(Status status)
    {
        var statusUI = statusUIPairs[status];
        statusUIPairs.Remove(status);
        statusUI.DestroyUI();
    }

   
}



// private Dictionary<string, ActiveStatus> activeEffects = new();
// private UnitController controller;
//private GameObject InstantiateIcon(StatusData status)
//{
//    var icon = Instantiate(StatusManager.Instance.IconPrefab, iconParent).GetComponent<StatusIcon>();
//    icon.ChangeIconSprite(status.IconSprite);
//    return icon.gameObject;
//}

//private StatusUI InstantiateTimer(float duration, StatusData status)
//{
//    var timer = Instantiate(StatusManager.Instance.TimerPrefab, timerParent).GetComponent<StatusUI>();
//    timer.Setup(duration, status.TimerColor);
//    return timer;
//}
//private void Awake()
//{
//    controller = GetComponent<UnitController>();
//}

//private void Start()
//{
//    controller.Health.OnDeathServer += EndAll;
//}

//public void EndAll()
//{
//   // foreach (var activeEffect in new List<ActiveStatus>(activeEffects.Values))
//    {
//   //     EndStatus(activeEffect.Effect.effectName);
//    }
//}

//public void EndStatus(string name)
//{
//    //if (activeEffects.ContainsKey(name))
//    //{
//    //    var activeEffect = activeEffects[name];
//    //    Destroy(activeEffect.IconObj.gameObject);
//    //    Destroy(activeEffect.Timer.gameObject);
//    //    activeEffects.Remove(name);
//    //}
//    else if (debug) Debug.Log("Disactive status tried to end : " + name + " userName : " + name);
//}

//public void StartStatus(string name, float time = 0)
//{
//   // var status = StatusEffectsData.Instance.GetStatusData(name);
//  //  StartCoroutine(StartStatus(status, time));
//}

//private IEnumerator StartStatus(StatusData status, float time = 0)
//{
//    //if (!activeEffects.ContainsKey(status.effectName))
//    //{
//    //    //controller.Health.OpenHealthBar();
//    //    ActiveStatus activeStatus = new ActiveStatus
//    //    {
//    //        Effect = status,
//    //        IconObj = InstantiateIcon(status),
//    //        Timer = InstantiateTimer(time, status)
//    //    };

//    //    activeEffects.Add(status.effectName, activeStatus);

//    //    yield return Extensions.GetWait(time);

//    //    EndStatus(status.effectName);
//    //}
//    //else if (debug) Debug.Log("Active status tried to start : " + status.effectName + " userName : " + name);
//}


//public void SetCanvasHeight(float height)
//{
//    Canvas.transform.localPosition = new Vector3(0, height, 0);
//}
//}
