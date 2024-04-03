using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopData : MonoBehaviour
{
    public List<Item> Stocks = new List<Item>();
    public List<Item> Stocks2 = new List<Item>();
    public List<Item> Stocks3 = new List<Item>();

    //여기서 아이템 추가하면 상점에 생김
    void Start()
    {
        //무기 상점
        Stocks.Add(ItemDataBase.instance.itemDB[0]);
        Stocks.Add(ItemDataBase.instance.itemDB[3]);


        //방패 상점
        Stocks2.Add(ItemDataBase.instance.itemDB[1]);
        Stocks2.Add(ItemDataBase.instance.itemDB[4]);


        //신발 상점
        Stocks3.Add(ItemDataBase.instance.itemDB[2]);
        Stocks3.Add(ItemDataBase.instance.itemDB[5]);
    }
}
