using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneLeft : MonoBehaviour
{
    private string filePath;
    private void Start()
    {
        filePath = Application.persistentDataPath + "/playerData.json";
    }

    public void GameSceneMove()
    {
        SceneManager.LoadScene("LoadingScene");
    }
    public void DeleteSave()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("JSON file deleted successfully.");
        }
        else
        {
            Debug.LogWarning("JSON file not found.");
        }
    }

}
