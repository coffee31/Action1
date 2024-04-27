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
                Instance = FindObjectOfType<Inventory>(); // 싱글톤 탐색

                if (Instance == null) // null이면 싱글톤 생성
                {
                    GameObject InvenSingle = new GameObject(typeof(Inventory).Name);
                    Instance = InvenSingle.AddComponent<Inventory>();
                }
            }
            return Instance;
        }
    }

    public InvenToryUI invenui;
    public int MaxSlotcount; //인벤토리 최대 저장 가능한 슬롯

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

    //슬롯 카운터 증가
    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    //슬롯에 아이템 추가
    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;

    private int slotCnt; //현재 활성화 시킨 슬롯
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
        MaxSlotcount = 30; // 위와 동일

    }

    //아이템 추가
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
            Debug.Log("아이템 삭제");
        }
        else
        {
            Debug.Log("아이템이 없습니다.");
        }
    }
}
