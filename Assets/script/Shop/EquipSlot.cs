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
        if (itemicon != null) // Null üũ
        {
            itemicon.sprite = item.Image;
            itemicon.gameObject.SetActive(true);
        }
    }

    public void UnequipItem()
    {
        if (equippedItem != null)
        {
            // ������ �ɷ�ġ �ʱ�ȭ �Ǵ� ���� ���� �۾� ����
            // ��: ������ �ɷ�ġ�� �ʱ�ȭ�ϰų� �ٸ� �۾� ����
            // ������ ������ ���� �ʱ�ȭ
            Debug.Log("������ ����: " + equippedItem.ItemName);

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
            Debug.Log("���� ������ �������� �����ϴ�.");
        }
    }
}
