using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMarble : MonoBehaviour
{
    [SerializeField] GameObject particle;
    GameObject marbleInstance;
    MainUI mainUI;

    private void Start()
    {
        mainUI = GameObject.FindWithTag("MainUI").GetComponent<MainUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Wall"))
        {
            marbleInstance = Instantiate(particle, transform.position, transform.rotation);
            marbleInstance.SetActive(true);
            StartCoroutine(particleOFF());
        }
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().CurHP -= (35 * GameDataManager.Instance.GameStage) - other.gameObject.GetComponent<Player>().DEF;
            mainUI.HPslider();
        }
    }

    IEnumerator particleOFF()
    {
        yield return new WaitForSeconds(0.12f);
        Destroy(marbleInstance);
        Destroy(gameObject);
    }
}
