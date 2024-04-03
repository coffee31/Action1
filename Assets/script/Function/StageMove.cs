using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageMove : MonoBehaviour
{
    [SerializeField] Transform StagePositon;
    [SerializeField] GameObject Text;
    [SerializeField] SoundManager SoundManager;
    float Timer;
    bool MoveStageON;

    private void Start()
    {
        Timer = 4.0f;
        MoveStageON = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !MoveStageON)
        {
            MoveStageON = true;
            StartCoroutine(Move(other.gameObject));
        }
    }

    IEnumerator Move(GameObject player)
    {
        while (Timer > 0)
        {
            Text.GetComponent<TMP_Text>().text = (int)Timer + "�ʵڿ� �÷��� ž���� �̵��մϴ�.";
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
        SoundManager.SoundChange();
        GameDataManager.Instance.GameStart = true;
        MoveStageON = false;
        Timer = 4.0f;

    }

}
