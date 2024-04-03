using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMgr : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;

    void Start()
    {
        StartCoroutine(LoadingScene ());
    }

    IEnumerator LoadingScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync("GameScene");
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;
            if(slider.value < 0.9f)
            {
                slider.value = Mathf.MoveTowards(slider.value, 0.9f,Time.deltaTime);
            }
            else if(operation.progress >= 0.9f)
            {
                slider.value = Mathf.MoveTowards(slider.value, 1f, Time.deltaTime);
            }

            if(slider.value >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

        }
            
    }

}
