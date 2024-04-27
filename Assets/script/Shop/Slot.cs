using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public int slotNum = 0;
    public Item item;
    public Image itemicon;
    public Image icon;
    public string imageBase64;


    GameObject equipParent;
    Player player;
    JsonData jsonData;

    private void Start()
    {
        jsonData = FindObjectOfType<JsonData>();
        equipParent = GameObject.FindWithTag("Equip");
        player = FindObjectOfType<Player>();
    }


    public void UpdateSlotUI()
    {
        if (item != null && itemicon != null) // Null 체크
        {
            itemicon.sprite = item.Image;
            itemicon.gameObject.SetActive(true);
        }
    }

    public void RemoveSlot()
    {
        item = null;
        itemicon.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Inventory.IsShopMode)
        {
            if (slotNum >= 0 && slotNum < Inventory.instance.items.Count)
            {
                player.Gold += (int)(item.Cost * 0.8f);
                Inventory.instance.RemoveItem(slotNum);
                GameDataManager.Instance.InvenText.GetComponent<TMP_Text>().text = "판매했습니다. 0_0";
                GameDataManager.Instance.InvenText.SetActive(true);
                StopCoroutine(TextOFF());
                StartCoroutine(TextOFF());
            }
            else
            {
                Debug.Log("유효하지 않은 슬롯 인덱스입니다.");
            }
        }
        else
        {
            if (slotNum >= 0 && slotNum < Inventory.instance.items.Count)
            {
                EquipON();
            }
            else
            {
                Debug.Log("유효하지 않은 슬롯 인덱스입니다.");
            }
        }
    }
    void EquipON()
    {
        if (equipParent == null)
        {
            Debug.Log("Equip 태그를 가진 부모 오브젝트를 찾을 수 없습니다.");
            return;
        }

        EquipSlot[] equipSlots = equipParent.GetComponentsInChildren<EquipSlot>(includeInactive: true);

        foreach (EquipSlot slot in equipSlots)
        {
            switch (item.itemClass)
            {
                case ItemClass.Sword:
                    
                    if (slot.CompareTag("weapon"))
                    {
                        slot.UpdateSlotUI(item);
                        Inventory.weaponON = true;

                        if (slot.equippedItem != null && slot.equippedItem.itemID == item.itemID)
                        {
                            GameDataManager.Instance.EquipText.GetComponent<TMP_Text>().text = "동일한 장비를 장착중입니다.";
                            GameDataManager.Instance.EquipText.SetActive(true);
                            StopCoroutine(TextOFF());
                            StartCoroutine(TextOFF());
                            return;
                        }

                        // 다른 아이템 ID인 경우 기존 아이템 능력치 해제
                        if (slot.equippedItem != null && slot.equippedItem.itemID != item.itemID)
                        {
                            slot.UnequipItem();
                            Inventory.weaponON = false;
                        }

                        // 새로운 아이템 장착 및 능력치 적용
                        slot.UpdateSlotUI(item);
                        Inventory.weaponON = true;
                        slot.equippedItem = item;
                        player.DMG += item.damage;
                        GameDataManager.Instance.EquipText.GetComponent<TMP_Text>().text = "무기를 장착했습니다.";
                        GameDataManager.Instance.EquipText.SetActive(true);
                        StopCoroutine(TextOFF());
                        StartCoroutine(TextOFF());
                    }
                    break;

                case ItemClass.Shield:

                    if (slot.CompareTag("shield"))
                    {
                        slot.UpdateSlotUI(item);
                        Inventory.ShieldON = true;

                        if (slot.equippedItem != null && slot.equippedItem.itemID == item.itemID)
                        {
                            GameDataManager.Instance.EquipText.GetComponent<TMP_Text>().text = "동일한 장비를 장착중입니다.";
                            GameDataManager.Instance.EquipText.SetActive(true);
                            StopCoroutine(TextOFF());
                            StartCoroutine(TextOFF());
                            return;
                        }


                        // 다른 아이템 ID인 경우 기존 아이템 능력치 해제
                        if (slot.equippedItem != null && slot.equippedItem.itemID != item.itemID)
                        {
                            slot.UnequipItem();
                            Inventory.ShieldON = false;
                        }

                        // 새로운 아이템 장착 및 능력치 적용
                        slot.UpdateSlotUI(item);
                        Inventory.ShieldON = true;
                        slot.equippedItem = item;
                        player.DEF += item.Def;
                        GameDataManager.Instance.EquipText.GetComponent<TMP_Text>().text = "방패를 장착했습니다.";
                        GameDataManager.Instance.EquipText.SetActive(true);
                        StopCoroutine(TextOFF());
                        StartCoroutine(TextOFF());
                    }
                    break;

                case ItemClass.Shoes:

                    if (slot.CompareTag("shoes"))
                    {
                        slot.UpdateSlotUI(item);
                        Inventory.ShoesON = true;

                        if (slot.equippedItem != null && slot.equippedItem.itemID == item.itemID)
                        {
                            GameDataManager.Instance.EquipText.GetComponent<TMP_Text>().text = "동일한 장비를 장착중입니다.";
                            GameDataManager.Instance.EquipText.SetActive(true);
                            StopCoroutine(TextOFF());
                            StartCoroutine(TextOFF());
                            return;
                        }

                        // 다른 아이템 ID인 경우 기존 아이템 능력치 해제
                        if (slot.equippedItem != null && slot.equippedItem.itemID != item.itemID)
                        {
                            slot.UnequipItem();
                            Inventory.ShoesON = false;
                        }

                        // 새로운 아이템 장착 및 능력치 적용
                        slot.UpdateSlotUI(item);
                        Inventory.ShoesON = true;
                        slot.equippedItem = item;
                        player.Speed += item.Speed;
                        GameDataManager.Instance.EquipText.GetComponent<TMP_Text>().text = "신발을 장착했습니다.";
                        GameDataManager.Instance.EquipText.SetActive(true);
                        StopCoroutine(TextOFF());
                        StartCoroutine(TextOFF());
                    } 
                    break;

                default:
                    Debug.Log("해당하는 아이템 클래스의 슬롯이 없습니다.");
                    break;
            }
        }
        Inventory.instance.RemoveItem(slotNum);
    }

    IEnumerator TextOFF()
    {
        yield return new WaitForSeconds(1);
        if (GameDataManager.Instance.EquipText.activeSelf)
        {
            GameDataManager.Instance.EquipText.SetActive(false);
        }
        if (GameDataManager.Instance.InvenText.activeSelf)
        {
            GameDataManager.Instance.InvenText.SetActive(false);
        }
    }
}
