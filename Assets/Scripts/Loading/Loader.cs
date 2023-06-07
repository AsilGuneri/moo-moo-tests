using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Loader : MonoBehaviour
{
    [SerializeField]private GameObject loadingScreen;
    [SerializeField]private TextMeshProUGUI progressText;
    [SerializeField]private Image progressBar;

    public void Load(AsyncOperation operation){
        loadingScreen.SetActive(true);
        StartCoroutine(LoadCoroutine(operation));
    }

    private IEnumerator LoadCoroutine(AsyncOperation operation){

        yield return new WaitUntil(() => operation != null);

        operation.allowSceneActivation = false;
        while(!operation.isDone){
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.fillAmount = progress;
            progressText.text = "Loading: " + (progress * 100f).ToString("f1") + "%";
        
            if(operation.progress >= 0.9f){
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
        
        Destroy(gameObject);
    }
}
