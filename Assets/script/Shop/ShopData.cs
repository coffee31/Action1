using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopData : MonoBehaviour
{
    public List<Item> Stocks = new List<Item>();
    public List<Item> Stocks2 = new List<Item>();
    public List<Item> Stocks3 = new List<Item>();

    //���⼭ ������ �߰��ϸ� ������ ����
    void Start()
    {
        //���� ����
        Stocks.Add(ItemDataBase.instance.itemDB[0]);
        Stocks.Add(ItemDataBase.instance.itemDB[3]);


        //���� ����
        Stocks2.Add(ItemDataBase.instance.itemDB[1]);
        Stocks2.Add(ItemDataBase.instance.itemDB[4]);


        //�Ź� ����
        Stocks3.Add(ItemDataBase.instance.itemDB[2]);
        Stocks3.Add(ItemDataBase.instance.itemDB[5]);
    }
}
