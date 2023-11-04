using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utilities;

public class NotificationManager : NetworkSingleton<NotificationManager>
{
    [SerializeField] GameObject notificationParent;
    [SerializeField] TextMeshProUGUI msgText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] int defaultTimer;

    [Server]
    public void SetNotification(string msg, int countdown = 0)
    {
        RpcSetNotification(msg, countdown);
    
    }
    [ClientRpc]
    private void RpcSetNotification(string msg, int countdown)
    {
        msgText.text = msg.ToUpper();
        StartCoroutine(NotificationRoutine(countdown));
    }

    private IEnumerator NotificationRoutine(int countdown)
    {
        bool useCountdown = countdown > 0;
        float time = useCountdown ? countdown : defaultTimer;
        notificationParent.SetActive(true);

        while (time > 0)
        {
            timerText.text = useCountdown ? time.ToString("F0") : "";
            yield return Extensions.GetWait(1f); // Assuming GetWait is a method that returns a new WaitForSeconds object
            time--;
        }
        notificationParent.SetActive(false);
    }


}
