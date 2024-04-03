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
                // ������ �̱��� ������Ʈ�� ã���ϴ�.
                instance = FindObjectOfType<GameManager>();

                // ���� �̱��� ������Ʈ�� ������ �����մϴ�.
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
