using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    [SerializeField]private GameObject loadingScreen;

    public void Load(AsyncOperation operation){
        Loader loader = Instantiate(loadingScreen, transform).GetComponent<Loader>();
        loader.Load(operation);
    }
}
