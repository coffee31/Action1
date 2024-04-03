using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Skill : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] SkillUI skillUI;
    [SerializeField] MainUI mainUI;
    [SerializeField] TrailRenderer Trail;

    [SerializeField] GameObject AreaQ;
    [SerializeField] GameObject AreaW;
    [SerializeField] GameObject AreaR;

    bool SkillQ = false;
    bool SkillW = false;
    bool SkillE = false;
    bool SkillR = false;
    bool SkillV = false;


    public float QcoolTime = 15;
    public float WcoolTime = 20;
    public float EcoolTime = 30;
    public float RcoolTime = 60;
    public float VcoolTime = 120;


    [SerializeField] GameObject EffectW;
    [SerializeField] GameObject EffectE;
    [SerializeField] GameObject EffectR;
    [SerializeField] GameObject EffectV;


    void Start()
    {
        player = GetComponent<Player>();
        SkillQ = false;
        SkillW = false;
        SkillE = false;
        SkillR = false;
        SkillV = false;
    }

    void Update()
    {
        UseSkill(KeyCode.Q, "Q");
        UseSkill(KeyCode.W, "W");
        UseSkill(KeyCode.E, "E");
        UseSkill(KeyCode.R, "R");
        UseSkill(KeyCode.V, "V");
    }


    void UseSkill(KeyCode skillKey, string skillName)
    {
        if (Input.GetKeyDown(skillKey) && !player.attacking && !player.SkillON && !player.isJump)
        {
            if (skillKey == KeyCode.Q && skillUI.QCool >= QcoolTime && player.CurMana >= 10)
            {
                player.CurMana -= 10;
                mainUI.MPslider();
                player.SkillON = true;
                SkillQ = true;
                skillUI.QCool = 0;
                skillUI.ImageQ.fillAmount = 0;
                player.animator.SetBool("Wind",true);
                StartCoroutine(SkillEffect());
                StartCoroutine(SkillOFF(1.2f));
            }
            if (skillKey == KeyCode.W && skillUI.WCool >= WcoolTime && player.CurMana >= 30)
            {
                player.CurMana -= 30;
                mainUI.MPslider();
                player.SkillON = true;
                SkillW = true;
                skillUI.WCool = 0;
                skillUI.ImageW.fillAmount = 0;
                player.animator.SetBool("Smite", true);
                StartCoroutine(SkillEffect());
                StartCoroutine(SkillOFF(2.68f));
            }
            if (skillKey == KeyCode.E && skillUI.ECool >= EcoolTime)
            {
                player.SkillON = true;
                SkillE = true;
                skillUI.ECool = 0;
                skillUI.ImageE.fillAmount = 0;
                player.animator.SetBool("Casting", true);
                StartCoroutine(SkillEffect());
                StartCoroutine(SkillOFF(2.54f));
            }
            if (skillKey == KeyCode.R && skillUI.RCool >= RcoolTime && player.CurMana >= 50)
            {
                player.CurMana -= 50;
                mainUI.MPslider();
                player.SkillON = true;
                SkillR = true;
                skillUI.RCool = 0;
                skillUI.ImageR.fillAmount = 0;
                player.animator.SetBool("Ultimate", true);
                StartCoroutine(SkillEffect());
                StartCoroutine(SkillOFF(3f));
            }
            if (skillKey == KeyCode.V && skillUI.VCool >= VcoolTime)
            {
                player.SkillON = true;
                SkillV = true;
                skillUI.VCool = 0;
                skillUI.ImageV.fillAmount = 0;
                player.animator.SetBool("Power", true);
                StartCoroutine(SkillEffect());
                StartCoroutine(SkillOFF(2.3f));
            }

        }
    }
    IEnumerator SkillOFF(float _wait)
    {
        yield return new WaitForSeconds(0.1f);
        if(player.animator.GetBool("Wind"))
            player.animator.SetBool("Wind", false);
        else if (player.animator.GetBool("Smite"))
            player.animator.SetBool("Smite", false);
        else if (player.animator.GetBool("Casting"))
            player.animator.SetBool("Casting", false);
        else if (player.animator.GetBool("Ultimate"))
            player.animator.SetBool("Ultimate", false);
        else if (player.animator.GetBool("Power"))
            player.animator.SetBool("Power", false);
        yield return new WaitForSeconds(_wait);
        player.SkillON = false;

    }
    IEnumerator SkillEffect()
    {
        if(SkillQ)
        {
            yield return new WaitForSeconds(0.2f);
            Trail.enabled = true;
            yield return new WaitForSeconds(0.5f);
            AreaQ.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            AreaQ.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            AreaQ.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            AreaQ.SetActive(false);
            Trail.enabled = false;
            SkillQ = false;
        }
        else if (SkillW)
        {
            EffectW.transform.position = new Vector3(player.transform.position.x, 0.5f, player.transform.position.z);
            EffectW.transform.rotation = Quaternion.Euler(player.transform.rotation.eulerAngles.x + 90, player.transform.rotation.eulerAngles.y - 90, player.transform.rotation.eulerAngles.z);
            
            yield return new WaitForSeconds(1.4f);
            EffectW.SetActive(true);
            AreaW.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            AreaW.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            EffectW.SetActive(false);
            SkillW = false;
        }
        else if (SkillE)
        {
            yield return null;
            EffectE.transform.position = player.transform.position;
            EffectE.SetActive(true);
            yield return new WaitForSeconds(1.8f);
            EffectE.SetActive(false);
            player.CurHP += (int)(player.MaxHP * 0.2f);
            player.CurMana += (int)(player.MaxMana * 0.2f);
            mainUI.HPslider();
            mainUI.MPslider();
            Debug.Log("È¸º¹");
            SkillE = false;
        }
        else if(SkillR)
        {
            yield return new WaitForSeconds(0.8f);
            EffectR.transform.position = player.transform.position;
            EffectR.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            AreaR.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            AreaR.SetActive(false);
            EffectR.SetActive(false);
            SkillR = false;
        }
        else if (SkillV)
        {
            EffectV.transform.position = player.transform.position;
            EffectV.SetActive(true);
            player.DMG *= 2;
            yield return new WaitForSeconds(1.0f);
            EffectV.SetActive(false);
            SkillR = false;
            yield return new WaitForSeconds(19.0f);
            player.DMG /= 2;
        }

    }
}
