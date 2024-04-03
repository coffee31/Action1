using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CateroyBtn : MonoBehaviour
{
    //�� ī�װ��� ����Ʈ�� ����
    //ī�װ� �߰��� �Ʒ����� UI�߰��ϰ� ON�϶��� Ȱ��ȭ ��Ű���
    public ScrollRect scrollRect;


    // ���� �巡�׿� rect ����
    public RectTransform[] categories;

    //ī�װ� �迭
    public GameObject[] categoryObjects;

    private int activeCategoryIndex = 0; // Ȱ��ȭ�� ī�װ��� �ε���

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
