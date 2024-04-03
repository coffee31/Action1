using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour, IPointerClickHandler
{
    public Image itemicon;
    [HideInInspector]
    public Item equippedItem;

    Player player;


    private void Start()
    {
        player = FindObjectOfType<Player>();
        if (equippedItem.itemtype == ItemType.None)
            equippedItem = null;
        if (!GameManager.SaveON)
            UnequipItem();
    }

    public void UpdateSlotUI(Item item)
    {
        if (itemicon != null) // Null 체크
        {
            itemicon.sprite = item.Image;
            itemicon.gameObject.SetActive(true);
        }
    }

    public void UnequipItem()
    {
        if (equippedItem != null)
        {
            // 아이템 능력치 초기화 또는 해제 관련 작업 수행
            // 예: 아이템 능력치를 초기화하거나 다른 작업 수행
            // 해제된 아이템 정보 초기화
            Debug.Log("아이템 해제: " + equippedItem.ItemName);

            Inventory.Instance.Additem(equippedItem);
            player.DMG -= equippedItem.damage;
            player.DEF -= equippedItem.Def;
            player.Speed -= equippedItem.Speed;
            equippedItem = null;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(equippedItem != null)
        {
            switch(equippedItem.itemClass)
            {
                case ItemClass.Sword:
                    Inventory.weaponON = false;
                    UnequipItem();
                    itemicon.gameObject.SetActive(false);
                    break;
                case ItemClass.Shield:
                    Inventory.ShieldON = false;
                    UnequipItem();
                    itemicon.gameObject.SetActive(false);
                    break;
                case ItemClass.Shoes:
                    Inventory.ShoesON = false;
                    UnequipItem();
                    itemicon.gameObject.SetActive(false);
                    break;
            }
        }
        else
        {
            Debug.Log("현재 장착된 아이템이 없습니다.");
        }
    }
}
