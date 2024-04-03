using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    
    // Start is called before the first frame update
    public static int Diamond = 0;
    public static bool SaveON = false;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                // 씬에서 싱글톤 오브젝트를 찾습니다.
                instance = FindObjectOfType<GameManager>();

                // 씬에 싱글톤 오브젝트가 없으면 생성합니다.
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(GameManager).Name);
                    instance = singletonObject.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        SaveON = false;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



}
