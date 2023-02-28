using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [SerializeField] Outline Outline;


    //when its on player, outline shows white for some reason
    private void OnMouseExit()
    {
        Outline.enabled = false;
    }
    private void OnMouseOver()
    {
        if (!Outline.enabled) Outline.enabled = true;
    }
}
