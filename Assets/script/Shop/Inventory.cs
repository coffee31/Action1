using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory Instance;
    public static Inventory instance
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<Inventory>(); // �̱��� Ž��

                if (Instance == null) // null�̸� �̱��� ����
                {
                    GameObject InvenSingle = new GameObject(typeof(Inventory).Name);
                    Instance = InvenSingle.AddComponent<Inventory>();
                }
            }
            return Instance;
        }
    }

    public InvenToryUI invenui;
    public int MaxSlotcount; //�κ��丮 �ִ� ���� ������ ����

    public static bool IsShopMode = false;

    public List<Item> items = new List<Item>();

    public static bool weaponON = false;
    public static bool ShieldON = false;
    public static bool ShoesON = false;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this; 
            DontDestroyOnLoad(this.gameObject);
        }
    }

    //���� ī���� ����
    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    //���Կ� ������ �߰�
    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;

    private int slotCnt; //���� Ȱ��ȭ ��Ų ����
    public int SlotCnt
    {
        get { return slotCnt; }
        set
        {
            slotCnt = value;
            onSlotCountChange?.Invoke(slotCnt);
        }
    }

    private void Start()
    {
        if(!GameManager.SaveON)
        {
            SlotCnt = 4;
        }
        MaxSlotcount = 30; // ���� ����

    }

    //������ �߰�
    public bool Additem(Item _item)
    {
        if (items.Count < MaxSlotcount)
        {
            items.Add(_item);
            if (onChangeItem != null)
                onChangeItem.Invoke();

            if (items.Count > SlotCnt && items.Count <= MaxSlotcount)
            {
                SlotCnt++;
                onSlotCountChange(SlotCnt);
            }

            return true;
        }
        return false;
    }

    public void RemoveItem(int index)
    {
        if (index >= 0 && index < items.Count)
        {
            items.RemoveAt(index);
            if (onChangeItem != null)
                onChangeItem.Invoke();

            if (SlotCnt > 4)
            {
                SlotCnt--;
                onSlotCountChange(SlotCnt);
            }
            Debug.Log("������ ����");
        }
        else
        {
            Debug.Log("�������� �����ϴ�.");
        }
    }
}
