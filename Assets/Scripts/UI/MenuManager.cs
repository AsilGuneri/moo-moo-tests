using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Menu [] menus;

    public static MenuManager instance;

    private void Awake() {

        if(instance != null){
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OpenMenu(string menuName){
        int length = menus.Length;
        for (int i = 0; i < length; i++)
        {
            if(menus[i].menuName == menuName){
                OpenMenu(menus[i]);
                return;
            }
        }
    }

    public void OpenMenu(Menu menu){
        int length = menus.Length;
        for (int i = 0; i < length; i++)
        {
            if(menus[i].isOpen)
                menus[i].Close();
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu){
        menu.Close();
    }
}
