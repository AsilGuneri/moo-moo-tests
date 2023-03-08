using UnityEngine;

public abstract class MyScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                T[] results = Resources.FindObjectsOfTypeAll<T>();
                if (results.Length == 0)
                {
                    Debug.LogError("MyScriptableSingleton: Could not find instance of type " + typeof(T).Name);
                    return null;
                }
                if (results.Length > 1)
                {
                    Debug.LogWarning("MyScriptableSingleton: Multiple instances of type " + typeof(T).Name + " found, using the first one.");
                }
                _instance = results[0];
            }
            return _instance;
        }
    }
}
