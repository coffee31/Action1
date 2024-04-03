using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] GameObject Option;
    [SerializeField] GameObject State;
    [SerializeField] GameObject SaveText;
    [SerializeField] Slider Slider_HP;
    [SerializeField] Slider Slider_MP;
    [SerializeField] Slider Slider_EXP;
    Player player;


    [SerializeField] TMP_Text text_atk;
    [SerializeField] TMP_Text text_def;
    [SerializeField] TMP_Text text_speed;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

    }


    private void Update()
    {
        updateList();
    }


    public void HPslider()
    {
        Slider_HP.value = player.CurHP / player.MaxHP;
    }
    public void MPslider()
    {
        Slider_MP.value = player.CurMana / player.MaxMana;
    }
    public void EXPslider()
    {
        Slider_EXP.value = player.Cur_EXP / player.Max_EXP;
    }


    void updateList()
    {
        text_atk.text = "ATK : " + player.DMG;
        text_def.text = "DEF : " + player.DEF;
        text_speed.text = "SPEED : " + player.Speed;
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (!State.activeSelf)
            {
                State.SetActive(true);
            }
            else
            {
                State.SetActive(false);
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Option.activeSelf)
            {
                Option.SetActive(true);
            }
            else
            {
                Option.SetActive(false);
            }
        }
    }

    public void OptionON()
    {
        if (!Option.activeSelf)
        {
            Option.SetActive(true);
        }
        else
        {
            Option.SetActive(false);
        }
    }

    public void Resume()
    {
        Option.SetActive(false);
    }

    public void StateUI()
    {
        if (!State.activeSelf)
        {
            State.SetActive(true);
        }
        else
        {
            State.SetActive(false);
        }
    }

    public void SaveTextON()
    {
        SaveText.SetActive(true);
        StartCoroutine(TextOFF());
    }

    IEnumerator TextOFF()
    {
        yield return new WaitForSeconds(1f);
        SaveText.SetActive(false);

    }
}
