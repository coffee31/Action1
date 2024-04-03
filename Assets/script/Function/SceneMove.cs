using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneMove : MonoBehaviour
{
    private string filePath;
    public JsonData jsonData;

    private void Start()
    {
        filePath = Application.persistentDataPath + "/playerData.json";
    }

    public void GameSceneMove()
    {
        SceneManager.LoadScene("LoadingScene");
    }
    public void MainSceneMove()
    {
        if(jsonData != null)
        {
            jsonData.SavePlayerData();
            SceneManager.LoadScene("MenuScene");
        }
    }



}
