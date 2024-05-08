using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossStageMove : MonoBehaviour
{
    [SerializeField] Transform StagePositon;
    [SerializeField] GameObject Text;
    [SerializeField] SoundManager SoundManager;
    [SerializeField] GameObject boss;
    [SerializeField] GameObject BossInfo;

    float Timer;

    private void Start()
    {
        Timer = 4.0f;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !GameManager.Instance.MoveStageON)
        {
            GameManager.Instance.MoveStageON = true;
            StartCoroutine(Move(other.gameObject));
        }
    }

    IEnumerator Move(GameObject player)
    {
        while (Timer > 0)
        {
            Text.GetComponent<TMP_Text>().text = (int)Timer + "초뒤에 보스방으로 이동합니다.";
            Text.SetActive(true);

            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                Timer = 0;
                Text.SetActive(false);
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<Rigidbody>().MovePosition(StagePositon.transform.position);
        yield return new WaitForSeconds(0.5f);
        boss.SetActive(true);
        BossInfo.SetActive(true);
        SoundManager.SoundChange3();
        GameDataManager.Instance.BossStart = true;
        GameManager.Instance.MoveStageON = false;
        Timer = 4.0f;
        StartCoroutine(TextOFF());
    }
    
    IEnumerator TextOFF()
    {
        yield return new WaitForSeconds(2);
        BossInfo.SetActive(false);
    }
    
}
