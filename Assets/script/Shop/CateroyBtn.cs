using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CateroyBtn : MonoBehaviour
{
    //각 카테고리의 리스트가 나옴
    //카테고리 추가시 아래에서 UI추가하고 ON일때만 활성화 시키면됨
    public ScrollRect scrollRect;


    // 상점 드래그용 rect 설정
    public RectTransform[] categories;

    //카테고리 배열
    public GameObject[] categoryObjects;

    private int activeCategoryIndex = 0; // 활성화된 카테고리의 인덱스

    private void Start()
    {
        ShowCategory(activeCategoryIndex);
    }

    public void OpenCategory(int index)
    {
        if (index >= 0 && index < categories.Length)
        {
            activeCategoryIndex = index;
            ShowCategory(activeCategoryIndex);
        }
    }

    private void ShowCategory(int index)
    {
        for (int i = 0; i < categories.Length; i++)
        {
            bool isActive = i == index;
            categoryObjects[i].SetActive(isActive);
        }

        scrollRect.content = categories[index];
    }
}
