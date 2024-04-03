using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase instance;

    public List<Item> itemDB = new List<Item>();

    //ItemManager���� ������ �߰��ϸ�� �׸��� �� �߰��� �������� ShopData���� ����ϸ��
    //ex) ������ �߰��� Stocks.Add(ItemDataBase.instance.itemDB[index]); �̷������� �߰�

    private void Awake()
    {
        instance = this;
    }
   
}
