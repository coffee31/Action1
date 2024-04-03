using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestFunction : MonoBehaviour
{
    [SerializeField] GameObject QuestPanel;
    [SerializeField] GameObject Yesbtn;
    [SerializeField] GameObject Infomation;
    [SerializeField] GameObject Text;
    [SerializeField] GameObject YesText;
    [SerializeField] Player player;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Text.GetComponent<TMP_Text>().text = GameDataManager.Instance.BossCount.ToString() + "/1";
            QuestPanel.SetActive(true);
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            QuestPanel.SetActive(false);
        }
    }

    public void YesButton()
    {
        if(GameDataManager.Instance.BossCount > 0)
        {
            GameDataManager.Instance.BossCount--;
            player.Gold += 1000;
            Text.GetComponent<TMP_Text>().text = GameDataManager.Instance.BossCount.ToString() + "/1";
            YesText.SetActive(true);
            StartCoroutine(TextOFF());
        }
    }

    IEnumerator TextOFF()
    {
        yield return new WaitForSeconds(1.5f);
        YesText.SetActive(false);
    }

}
