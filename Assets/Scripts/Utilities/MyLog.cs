using UnityEngine;
using System.Collections;

public class MyLog : MonoBehaviour
{
    string myLog;
    Queue myLogQueue = new Queue();

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        myLog = logString;
        string newString = "\n [" + type + "] : " + myLog;
        myLogQueue.Enqueue(newString);
        if (type == LogType.Exception)
        {
            newString = "\n" + stackTrace;
            myLogQueue.Enqueue(newString);
        }
        myLog = string.Empty;
        foreach (string mylog in myLogQueue)
        {
            myLog += mylog;
        }
    }

    public void ClearQueue()
    {
        myLog = "";
        myLogQueue.Clear();
    }

    void OnGUI()
    {
        GUIStyle s = new GUIStyle();
        s.fontSize = 16;
        s.normal.textColor = Color.green;
        GUI.Label(new Rect(50, 50, Screen.width, Screen.height - 50), myLog, s);

    }
}