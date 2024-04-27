using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenToryUI : MonoBehaviour
{
    Inventory inven;
    public GameObject InventoryUI;
    public GameObject ShopUI;
    bool ActiveInven = false;

    Item item;

    //�κ��丮 ����
    public Slot[] slots;
    public Transform slotHolder;

    //�� ī�װ��� ����
    public ShopSlot[] Swordslots;
    public Transform SwordHolder;

    public ShopSlot[] Shieldslots;
    public Transform ShieldHolder;

    public ShopSlot[] Shoesslots;
    public Transform ShoesHolder;


    void Start()
    {
        inven = Inventory.instance;
        InventoryUI.SetActive(ActiveInven);
        slots = slotHolder.GetComponentsInChildren<Slot>();
        Swordslots = SwordHolder.GetComponentsInChildren<ShopSlot>();
        Shieldslots = ShieldHolder.GetComponentsInChildren<ShopSlot>();
        Shoesslots = ShoesHolder.GetComponentsInChildren<ShopSlot>();

        inven.onSlotCountChange += SlotUP;
        inven.onChangeItem += ChangeSlot;

        ChangeSlot(); // �κ��丮 ������ ���� �ʱ�ȭ

    }

    public void SlotUP(int val)
    {
        //���� �þ ������ ��ư Ȱ��ȭ
        for (int i = 0; i < slots.Length; i++) 
        {
            slots[i].slotNum = i;

            if (i < inven.SlotCnt)
                slots[i].GetComponent<Button>().interactable = true;
            else
                slots[i].GetComponent<Button>().interactable = false;
        }
    }

    void SlotUpdate()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotNum = i;

            if (i < inven.SlotCnt)
                slots[i].GetComponent<Button>().interactable = true;
            else
                slots[i].GetComponent<Button>().interactable = false;
        }
    }

    void Update()
    {
        //�κ��丮 â ����Ű
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!ActiveInven)
            {
                ActiveInven = true;
                InventoryUI.SetActive(ActiveInven);
                SlotUpdate();
                ChangeSlot();

            }
            else
            {
                ActiveInven = false;
                InventoryUI.SetActive(ActiveInven);
                GameDataManager.Instance.InvenText.SetActive(false);
                GameDataManager.Instance.EquipText.SetActive(false);

            }
        }
    }

    public void ChangeSlot()
    {
        int itemCount = Mathf.Min(slots.Length, inven.items.Count);

        for (int i = 0; i < itemCount; i++)
        {
            slots[i].item = inven.items[i];
            slots[i].UpdateSlotUI();            
        }

        // ���� ���� �ʱ�ȭ
        for (int i = itemCount; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();
        }
    }
}
