using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using static UnityEditor.Progress;


public class PlayerData
{
    //�÷��̾� ����
    public float MaxHP;
    public float CurHP;
    public float MaxMana;
    public float CurMana;
    public int DMG;
    public int DEF;
    public int Gold;
    public int Level;
    public int Speed;
    public float Cur_EXP;
    public float Max_EXP;
    public int Diamond;

    //���� ����
    public int GameStage;
    public int BossCount;


    //�κ��丮
    public List<Item> items;
    public int SlotCnt;
    public bool SaveON;

    // ���� ������ ����
    public Item equippedWeapon;
    // ���� ������ ����
    public Item equippedShield;
    // ���� ������ �Ź�
    public Item equippedShoes;


    public bool weaponON;
    public bool ShieldON;
    public bool ShoesON;

    // ���� ������ ���� �̹���
    public string equippedWeaponImage;
    // ���� ������ ���� �̹���
    public string equippedShieldImage;
    // ���� ������ �Ź� �̹���
    public string equippedShoesImage;



    public List<string> itemImages;
}

public class JsonData : MonoBehaviour
{
    public PlayerData playerData;
    public Player player;
    public GameDataManager gameDataManager;
    public Inventory inventory;


    private string playerDataPath;
    void Start()
    {
        playerData = new PlayerData();
        playerDataPath = Application.persistentDataPath + "/playerData.json";
    }

    public void SavePlayerData()
    {
        GameManager.SaveON = true;
        // PlayerData ��ü ���� �� ������ ����
        
        playerData.MaxHP = player.MaxHP;
        playerData.CurHP = player.CurHP;
        playerData.MaxMana = player.MaxMana;
        playerData.CurMana = player.CurMana;
        playerData.DMG = player.DMG;
        playerData.DEF = player.DEF;
        playerData.Gold = player.Gold;
        playerData.Level = player.Level;
        playerData.Cur_EXP = player.Cur_EXP;
        playerData.Max_EXP = player.Max_EXP;
        
        playerData.Speed = player.Speed;
        playerData.GameStage = gameDataManager.GameStage;
        playerData.BossCount = gameDataManager.BossCount;
        playerData.Diamond = GameManager.Diamond;
        playerData.SaveON = GameManager.SaveON;
        //�κ��丮
        playerData.items = inventory.items;
        playerData.SlotCnt = inventory.SlotCnt;

        playerData.weaponON = Inventory.weaponON;
        playerData.ShieldON = Inventory.ShieldON;
        playerData.ShoesON = Inventory.ShoesON;

        // ���� ������ ���� ����
        playerData.equippedWeapon = FindEquippedItem(ItemClass.Sword);
        // ���� ������ ���� ����
        playerData.equippedShield = FindEquippedItem(ItemClass.Shield);
        // ���� ������ �Ź� ����
        playerData.equippedShoes = FindEquippedItem(ItemClass.Shoes);

        playerData.equippedWeaponImage = playerData.equippedWeapon != null ? TextureToBase64(playerData.equippedWeapon.Image.texture) : "";
        playerData.equippedShieldImage = playerData.equippedShield != null ? TextureToBase64(playerData.equippedShield.Image.texture) : "";
        playerData.equippedShoesImage = playerData.equippedShoes != null ? TextureToBase64(playerData.equippedShoes.Image.texture) : "";


        //�κ��丮 �̹��� ����
        playerData.itemImages = new List<string>();
        foreach (Item item in inventory.items)
        {
            string base64Image = TextureToBase64(item.Image.texture);
            playerData.itemImages.Add(base64Image);
        }


        // PlayerData ��ü�� JSON ���ڿ��� ��ȯ
        string jsonData = JsonConvert.SerializeObject(playerData);

        // JSON �����͸� ���Ͽ� ����
        File.WriteAllText(playerDataPath, jsonData);

        Debug.Log("Player data saved.");

    }
    public PlayerData LoadPlayerData()
    {
        // ����� JSON ������ �о��
        if (File.Exists(playerDataPath))
        {
            string jsonData = File.ReadAllText(playerDataPath);
            // JSON ���ڿ��� PlayerData ��ü�� ������ȭ
            PlayerData loadedPlayerData = JsonConvert.DeserializeObject<PlayerData>(jsonData);

            // �̹��� ����
            for (int i = 0; i < loadedPlayerData.items.Count; i++)
            {
                string base64Image = loadedPlayerData.itemImages[i];
                Texture2D texture = Base64ToTexture(base64Image);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                loadedPlayerData.items[i].Image = sprite;
            }


            // ���� ������ ������ ����
            SetEquippedItem(loadedPlayerData.equippedWeapon);
            SetEquippedItem2(loadedPlayerData.equippedShield);
            SetEquippedItem3(loadedPlayerData.equippedShoes);


            if (!string.IsNullOrEmpty(loadedPlayerData.equippedWeaponImage))
            {
                Texture2D weaponTexture = Base64ToTexture(loadedPlayerData.equippedWeaponImage);
                Sprite weaponSprite = Sprite.Create(weaponTexture, new Rect(0, 0, weaponTexture.width, weaponTexture.height), Vector2.one * 0.5f);
                loadedPlayerData.equippedWeapon.Image = weaponSprite;
            }

            if (!string.IsNullOrEmpty(loadedPlayerData.equippedShieldImage))
            {
                Texture2D shieldTexture = Base64ToTexture(loadedPlayerData.equippedShieldImage);
                Sprite shieldSprite = Sprite.Create(shieldTexture, new Rect(0, 0, shieldTexture.width, shieldTexture.height), Vector2.one * 0.5f);
                loadedPlayerData.equippedShield.Image = shieldSprite;
            }

            if (!string.IsNullOrEmpty(loadedPlayerData.equippedShoesImage))
            {
                Texture2D shoesTexture = Base64ToTexture(loadedPlayerData.equippedShoesImage);
                Sprite shoesSprite = Sprite.Create(shoesTexture, new Rect(0, 0, shoesTexture.width, shoesTexture.height), Vector2.one * 0.5f);
                loadedPlayerData.equippedShoes.Image = shoesSprite;
            }

            foreach (EquipSlot slot in FindObjectsOfType<EquipSlot>())
            {
                if (slot.equippedItem != null)
                {
                    slot.UpdateSlotUI(slot.equippedItem);
                }
            }


            return loadedPlayerData;
        }
        else
        {
            Debug.LogWarning("Player data file not found.");
            return null;
        }
    }

    private Item FindEquippedItem(ItemClass itemClass)
    {
        foreach (EquipSlot slot in GameObject.FindWithTag("Equip").GetComponentsInChildren<EquipSlot>(includeInactive: true))
        {
            if (slot.equippedItem != null && slot.equippedItem.itemClass == itemClass)
            {
                return slot.equippedItem;
            }
        }
        return null;
    }

    // Ư�� ��� Ŭ������ ���� ������ �������� �����ϴ� �޼���
    private void SetEquippedItem(Item item)
    {
        foreach (EquipSlot slot in GameObject.FindWithTag("Equip").GetComponentsInChildren<EquipSlot>(includeInactive: true))
        {
            if (slot.equippedItem == null && slot.CompareTag("weapon"))
            {
                slot.equippedItem = item;
                break;
            }
        }
    }
    private void SetEquippedItem2(Item item)
    {
        foreach (EquipSlot slot in GameObject.FindWithTag("Equip").GetComponentsInChildren<EquipSlot>(includeInactive: true))
        {
            if (slot.equippedItem == null && slot.CompareTag("shield"))
            {
                slot.equippedItem = item;

                break;
            }
        }
    }
    private void SetEquippedItem3(Item item)
    {
        foreach (EquipSlot slot in GameObject.FindWithTag("Equip").GetComponentsInChildren<EquipSlot>(includeInactive: true))
        {
            if (slot.equippedItem == null && slot.CompareTag("shoes"))
            {
                slot.equippedItem = item;
                break;
            }
        }
    }
    public string TextureToBase64(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        string base64String = System.Convert.ToBase64String(bytes);
        return base64String;
    }

    // Base64 ���ڿ��� �̹����� ��ȯ�Ͽ� ��ȯ
    public Texture2D Base64ToTexture(string base64String)
    {
        byte[] bytes = System.Convert.FromBase64String(base64String);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        return texture;
    }
}
