using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] GameObject BossPos;

    // Update is called once per frame
    private void OnEnable()
    {
        gameObject.transform.position = BossPos.transform.position;
    }
}
