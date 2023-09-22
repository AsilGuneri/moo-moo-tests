using UnityEngine;
using System.Collections.Generic;

public class UnityLogDisplay : MonoBehaviour
{
    private bool showLogs = false;
    private List<string> logs = new List<string>();
    private Vector2 scrollPosition;

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
        if (type == LogType.Warning) return;
        logs.Add(logString);
        // Optionally, you can also capture the stack trace or log type if needed.
        // For example:
        // logs.Add(logString + "\n" + stackTrace);
    }

    void OnGUI()
    {
        // Define the area for the GUI elements in the top right corner
        GUILayout.BeginArea(new Rect(Screen.width - 520, 10, 510, 320));

        if (GUILayout.Button("Toggle Logs"))
        {
            showLogs = !showLogs;
        }

        if (showLogs)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, "box", GUILayout.Width(500), GUILayout.Height(300));
            foreach (string log in logs)
            {
                GUILayout.Label(log);
            }
            GUILayout.EndScrollView();
        }

        GUILayout.EndArea();
    }
}
