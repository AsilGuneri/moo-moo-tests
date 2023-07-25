using UnityEditor;
using UnityEngine;

public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (!_instance)
            {
                T[] resulsts = Resources.LoadAll<T>("Scriptable Objects/Singletons");
                if (resulsts.Length != 1) { Debug.LogError($"There are more than 1, or 0 {typeof(T).Name}"); return null; }
                _instance = resulsts[0];
                _instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
            }
            return _instance;
        }
    }
}