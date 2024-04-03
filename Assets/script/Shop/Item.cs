using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//������ ������ ������ �ڵ�
[System.Serializable]
public enum ItemType
{
    Equipment,
    Consumables,
    ETC,
    None
}

[System.Serializable]
public enum ItemClass
{
    Sword,
    Shield,
    Shoes,
    None
}


[System.Serializable]
public class Item
{
    public int itemID;
    public ItemType itemtype;
    public ItemClass itemClass;
    public string ItemName;

    [JsonIgnore]
    public Sprite Image;

    public string imageBase64;

    public int damage;
    public int Cost;
    public int Def;
    public int Speed;

    public void SaveImage(Sprite sprite)
    {
        imageBase64 = ImageSave.SpriteToBase64(Image);
    }

    // Base64 ���ڿ��� Sprite�� ��ȯ�Ͽ� ��ȯ
    public Sprite LoadImage()
    {
        if (!string.IsNullOrEmpty(imageBase64))
        {
            return ImageSave.Base64ToSprite(imageBase64);
        }
        else
        {
            return null;
        }
    }
}

