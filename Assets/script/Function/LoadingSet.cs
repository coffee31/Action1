using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSet : MonoBehaviour
{
    public InvenToryUI invenui;
    [SerializeField] MainUI mainUI;
    public JsonData data;
    Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
        data = FindObjectOfType<JsonData>();
        mainUI = FindObjectOfType<MainUI>();
        LateLoadData();

    }

    void LateLoadData()
    {
        PlayerData loadedPlayerData = data.LoadPlayerData();

        if (loadedPlayerData != null)
        {
            Inventory.Instance.items = loadedPlayerData.items;
            Inventory.Instance.SlotCnt = loadedPlayerData.SlotCnt;

            Inventory.weaponON = loadedPlayerData.weaponON;
            Inventory.ShieldON = loadedPlayerData.ShieldON;
            Inventory.ShoesON = loadedPlayerData.ShoesON;

            mainUI.HPslider();
            mainUI.MPslider();
            mainUI.EXPslider();
            Debug.Log("Player data Late loaded.");

        }
        else
        {
            Debug.LogWarning("Failed to load player data.");
        }
    }



}
