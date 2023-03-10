using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Utilities;

public class LobbyUI : NetworkSingleton<LobbyUI>
{
    [SerializeField] private GameObject playerFramePrefab;
    [SerializeField] private Transform contentParent;

    public GameObject SpawnPlayerFrame()
    {
        return Instantiate(playerFramePrefab, contentParent);
    }

}
