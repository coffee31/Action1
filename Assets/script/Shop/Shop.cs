using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject shopUI;
    public ShopData shopdata;
    public InvenToryUI invenToryUI;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Inventory.IsShopMode = true;
            shopUI.SetActive(true);
            ResetAllSlots();

            // Sword ���� ����
            SetSlotItems(shopdata.Stocks, invenToryUI.Swordslots);

            // Shield ���� ����
            SetSlotItems(shopdata.Stocks2, invenToryUI.Shieldslots);

            // Shoes ���� ����
            SetSlotItems(shopdata.Stocks3, invenToryUI.Shoesslots);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory.IsShopMode = false;
            shopUI.SetActive(false);
            GameDataManager.Instance.ShopText.SetActive(false);
        }
    }
    private void ResetAllSlots()
    {
        ResetSlots(invenToryUI.Swordslots);
        ResetSlots(invenToryUI.Shieldslots);
        ResetSlots(invenToryUI.Shoesslots);
    }

    
    private void ResetSlots(ShopSlot[] slots)
    {
        foreach (var slot in slots)
        {
            slot.item = null;
            slot.UpdateSlotUI();
        }
    }

    //������ �������� �ִ� ��쿡�� ��ư Ȱ��ȭ
    private void SetSlotItems(List<Item> items, ShopSlot[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count)
            {
                slots[i].item = items[i];
                slots[i].UpdateSlotUI();
                slots[i].GetComponent<Button>().interactable = true; // �������� ������ ��ȣ �ۿ� �����ϵ��� ����
            }
            else
            {
                slots[i].item = null;
                slots[i].UpdateSlotUI();
                slots[i].GetComponent<Button>().interactable = false; // �������� ������ ��ȣ �ۿ� �Ұ����ϵ��� ����
            }
        }
    }
}
