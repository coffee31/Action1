using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class EnemyBar : MonoBehaviour
{
    Transform Targettransform;
    RectTransform rectTransform;
    Image image;

    GameObject targetMonster;


    public void Setup(GameObject target)
    {
        image = GetComponent<Image>();
        Targettransform = target.transform;
        targetMonster = target;
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame

    private void Update()
    {
        image.fillAmount = targetMonster.GetComponent<Enemy>().CurHP / targetMonster.GetComponent<Enemy>().MaxHP;
    }
    void LateUpdate()
    {
        if(Targettransform.gameObject.activeSelf == false)
        {
            gameObject.SetActive(false);
        }
        transform.position = Targettransform.position + Vector3.up; // 몬스터의 위로 조정

        
        

    }
}
