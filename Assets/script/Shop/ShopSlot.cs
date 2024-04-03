using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour, IPointerClickHandler
{
    Inventory inven;

    public Item item;
    public Image itemicon;
    Player player;

    public int itemCount = 0;

    public InvenToryUI invenToryUI;

    //�κ��丮 ���԰� �����ϰ� UI ������Ʈ��
    private void Start()
    {
        inven = Inventory.Instance;
        player = FindObjectOfType<Player>();
        UpdateSlotUI();
    }

    public void UpdateSlotUI()
    {
        if (item != null)
        {
            itemicon.sprite = item.Image;
            itemicon.gameObject.SetActive(true);
        }
        else
        {
            itemicon.gameObject.SetActive(false);
        }
    }

    public void RemoveSlot()
    {
        item = null;
        itemicon.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(item != null)
        {
            if (inven.items.Count < inven.MaxSlotcount)
            {
                if(player.Gold >= item.Cost)
                {
                    bool added = inven.Additem(item);
                    if (added)
                    {
                        player.Gold -= item.Cost;
                        GameDataManager.Instance.ShopText.GetComponent<TMP_Text>().text = "�����߽��ϴ�.";
                        GameDataManager.Instance.ShopText.SetActive(true);
                        StopCoroutine(TextOFF());
                        StartCoroutine(TextOFF());
                    }
                }
                else
                {
                    GameDataManager.Instance.ShopText.GetComponent<TMP_Text>().text = "���� �����մϴ�.";
                    GameDataManager.Instance.ShopText.SetActive(true);
                    StopCoroutine(TextOFF());
                    StartCoroutine(TextOFF());
                }
            }
            else
            {
                GameDataManager.Instance.ShopText.GetComponent<TMP_Text>().text = "����ǰ�� ����á���ϴ�.";
                GameDataManager.Instance.ShopText.SetActive(true);
                StopCoroutine(TextOFF());
                StartCoroutine(TextOFF());
            }
        }
    }

    IEnumerator TextOFF()
    {
        yield return new WaitForSeconds(1);
        if(GameDataManager.Instance.ShopText.activeSelf)
        {
            GameDataManager.Instance.ShopText.SetActive(false);
        }

    }
}
