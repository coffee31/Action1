using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingArea : MonoBehaviour
{
    private MainUI mainUI;

    private void Start()
    {
        mainUI = FindObjectOfType<MainUI>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().CurHP = other.gameObject.GetComponent<Player>().MaxHP;
            other.gameObject.GetComponent<Player>().CurMana = other.gameObject.GetComponent<Player>().MaxMana;

            mainUI.SliderRefresh();

            Debug.Log("회복되었습니다.");
        }
    }
}
