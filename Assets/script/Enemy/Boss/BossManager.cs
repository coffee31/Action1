using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    [SerializeField] float BossTimer;
    [SerializeField] GameObject BossChar;

    [SerializeField] GameObject BossPanel;
    [SerializeField] Image image;
    [SerializeField] TMP_Text TimeText;
    [SerializeField] TMP_Text BossHP;


    Enemy Enemy;
    private void Start()
    {
        BossTimer = 120f;
        Enemy = BossChar.GetComponent<Enemy>();
    }

    private void Update()
    {
        if (GameDataManager.Instance.BossStart)
        {
            BossPanel.SetActive(true);
            BossTimer -= Time.deltaTime;
            TimeText.text = "남은 시간 : " + (int)BossTimer + "초";
            BossHP.text = Enemy.CurHP + "/" + Enemy.MaxHP;
            image.fillAmount = Enemy.CurHP / Enemy.MaxHP;
        }
            

        if(BossTimer <= 0)
        {
            GameDataManager.Instance.BossStart = false;
            GameDataManager.Instance.Respawn();
            BossTimer = 120f;
            BossChar.SetActive(false);
        }
        else if(!GameDataManager.Instance.BossStart)
        {
            BossPanel.SetActive(false);
            BossTimer = 120f;
        }
            
    }

}
