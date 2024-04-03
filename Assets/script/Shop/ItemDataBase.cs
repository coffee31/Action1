using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase instance;

    public List<Item> itemDB = new List<Item>();

    //ItemManager에서 아이템 추가하면됨 그리고 이 추가된 아이템을 ShopData에서 사용하면됨
    //ex) 아이템 추가후 Stocks.Add(ItemDataBase.instance.itemDB[index]); 이런식으로 추가

    private void Awake()
    {
        instance = this;
    }
   
}
