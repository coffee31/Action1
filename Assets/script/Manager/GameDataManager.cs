using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements.Experimental;

public class GameDataManager : MonoBehaviour
{
    private static GameDataManager instance;

    public bool GameStart = false;
    public bool BossStart = false;


    public int GameStage = 1;
    public int BossCount = 0;


    [SerializeField] Transform trans;
    [SerializeField] Player player;
    [SerializeField] SoundManager soundManager;
    [SerializeField] GameObject Text;
    [SerializeField] MainUI mainUI;
    [SerializeField] GameObject BossSet;
    [SerializeField] GameObject SmitePos;


    public InvenToryUI invenUI;

    public GameObject MagicAttack;
    public GameObject SmiteEffect;
    public GameObject SmiteRange;



    //Text
    public GameObject ShopText;
    public GameObject InvenText;
    public GameObject EquipText;
   
    public JsonData jsonData;



    public static GameDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                // 씬에서 싱글톤 오브젝트를 찾습니다.
                instance = FindObjectOfType<GameDataManager>();

                // 씬에 싱글톤 오브젝트가 없으면 생성합니다.
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(GameDataManager).Name);
                    instance = singletonObject.AddComponent<GameDataManager>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        player = FindObjectOfType<Player>();
        GameStart = false;
        LoadData();

    }



    public void Respawn()
    {
        if (BossStart)
        {
            BossSet.SetActive(false);
        }
        MagicAttack.SetActive(false);
        SmiteEffect.SetActive(false);
        SmitePos.SetActive(false);
        SmiteRange.SetActive(false);
        SmiteRange.transform.localScale = Vector3.one;
        SmiteRange.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0.02f);
        player.move = false;
        GameStart = false;
        BossStart = false;
        Text.GetComponent<TMP_Text>().text = "1초뒤에 마을로 이동합니다.";
        Text.SetActive(true);
        StartCoroutine(posmove());


    }

    IEnumerator posmove()
    {
        yield return new WaitForSeconds(1.5f);
        Text.SetActive(false);
        player.rigid.MovePosition(trans.transform.position);
        soundManager.SoundChange2();
        yield return new WaitForSeconds(1f);
        player.CurHP = player.MaxHP;
        player.CurMana = player.MaxMana;
        mainUI.HPslider();
        mainUI.MPslider();
    }

    void LoadData()
    {
        // JsonData 클래스의 인스턴스를 통해 플레이어 데이터를 불러옴
        PlayerData loadedPlayerData = jsonData.LoadPlayerData();

        if (loadedPlayerData != null)
        {
            GameManager.SaveON = loadedPlayerData.SaveON;
            
            // 불러온 플레이어 데이터로 플레이어 객체의 상태를 업데이트
            
            player.MaxHP = loadedPlayerData.MaxHP;
            player.CurHP = loadedPlayerData.CurHP;
            player.MaxMana = loadedPlayerData.MaxMana;
            player.CurMana = loadedPlayerData.CurMana;
            player.DMG = loadedPlayerData.DMG;
            player.DEF = loadedPlayerData.DEF;
            player.Gold = loadedPlayerData.Gold;
            player.Level = loadedPlayerData.Level;
            player.Speed = loadedPlayerData.Speed;
            player.Cur_EXP = loadedPlayerData.Cur_EXP;
            player.Max_EXP = loadedPlayerData.Max_EXP;
            
            GameStage = loadedPlayerData.GameStage;
            BossCount = loadedPlayerData.BossCount;
            GameManager.Diamond = loadedPlayerData.Diamond;


            Debug.Log("Player data loaded.");
        }
        else
        {
            Debug.LogWarning("Failed to load player data.");
        }

    }
}
