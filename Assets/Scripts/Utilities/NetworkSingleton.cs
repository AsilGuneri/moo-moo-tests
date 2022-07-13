using UnityEngine;
using Mirror;

namespace Utilities
{
    public class NetworkSingleton<T> : NetworkBehaviour
        where T : NetworkBehaviour
    {
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    _instance = new GameObject("Instance of" + typeof(T)).AddComponent<T>();
                }
                return _instance;
            }
        }
    }
}