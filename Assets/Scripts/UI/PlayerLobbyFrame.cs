using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLobbyFrame : NetworkBehaviour
{
    [SerializeField] Button[] buttonsToEnable;

    private void Awake()
    {
        foreach(var button in buttonsToEnable) 
        {
            if (button.interactable)
            {
                button.interactable = false;
            }
        }
    }
    public void OnAuthorityAssigned()
    {
        if (hasAuthority)
        {
            foreach(var button in buttonsToEnable)
            {
                button.interactable = true;
            }
        }

    }
}
