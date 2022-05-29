using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomNetworkPlayer : NetworkBehaviour
{


    [SyncVar(hook = nameof(HandleDisplayNameUpdated))]
    [SerializeField] private string displayName = "Missing Name";
    [SerializeField] private TMP_Text nameText;

    #region Server
    [Server] 
    public void SetDisplayName(string name)
    {
        displayName = name;
    }
    
    [Command] //Clients calling a method on server
    private void CmdSetDisplayName(string newDisplayName) 
    {
        if (newDisplayName.Length < 2 || newDisplayName.Length > 20) { return; } //Server authorization
        RpcLogNewName(newDisplayName);
        SetDisplayName(newDisplayName);
    }


    #endregion
    #region Client
    private void HandleDisplayNameUpdated(string oldName, string newName)
    {
        nameText.text = newName;
    }

    [ContextMenu("Set name")]
    private void SetMyName()
    {
        CmdSetDisplayName("new name");
    }
    [ClientRpc] //Server calling a method on all clients
    private void RpcLogNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }
    #endregion
}
