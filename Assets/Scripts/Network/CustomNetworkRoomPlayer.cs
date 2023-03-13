using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNetworkRoomPlayer : NetworkRoomPlayer
{


    CustomNetworkRoomManager CustomManager;

    private void Awake()
    {
        CustomManager = NetworkRoomManager.singleton as CustomNetworkRoomManager;
    }
    private void OnEnable()
    {
        transform.SetParent(GameObject.Find("PlayerObjects").transform);
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        CustomManager.RoomPlayers.Add(this);
        //StartCoroutine(tst());
    }
    public override void OnStopClient()
    {
        base.OnStopClient();
        CustomManager.RoomPlayers.Remove(this);

    }

    //private IEnumerator tst()
    //{
    //    yield return new WaitForSeconds(5);
    //    LobbyManager.Instance.UpdatePlayerItems();
    //}

    //[SyncVar(hook = nameof(OnParentChanged))]
    //private NetworkIdentity parentIdentity;

    //private void OnParentChanged(NetworkIdentity oldParent, NetworkIdentity newParent)
    //{
    //    if (newParent != null)
    //    {
    //        transform.SetParent(newParent.transform);
    //    }
    //    else
    //    {
    //        transform.SetParent(null);
    //    }
    //}

    //public void CmdSetParent(NetworkIdentity newParent)
    //{
    //    if (parentIdentity == newParent) return;
    //    parentIdentity = newParent;
    //}

}
