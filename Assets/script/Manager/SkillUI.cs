using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading;

public class SkillUI : MonoBehaviour
{
    Player player;
    Weapon weapon;
    Skill skill;


    [SerializeField] TMP_Text HP;
    [SerializeField] TMP_Text MP;
    [SerializeField] TMP_Text LV;
    [SerializeField] TMP_Text EXP;
    [SerializeField] TMP_Text Gold;
    [SerializeField] TMP_Text Diamond;

    public Image Image1;
    public Image Image2;
    public Image ImageQ;
    public Image ImageW;
    public Image ImageE;
    public Image ImageR;
    public Image ImageV;

    public float CoolTime;
    public float DashCool;

    public float QCool;
    public float WCool;
    public float ECool;
    public float RCool;
    public float VCool;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        weapon = GameObject.FindWithTag("Melee").GetComponent<Weapon>();
        skill = GameObject.FindWithTag("Player").GetComponent<Skill>();
        InitCool();

    }
    void Update()
    {
        PlayerStateUI();
        AttackCool();
        DashTime();

    }
    private void FixedUpdate()
    {
        SkillCool();
    }
    void InitCool()
    {
        CoolTime = 1; 
        DashCool = 7;
        QCool = skill.QcoolTime;
        WCool = skill.WcoolTime;
        ECool = skill.EcoolTime;
        RCool = skill.RcoolTime;
        VCool = skill.VcoolTime;
    }

    void PlayerStateUI()
    {
        HP.text = player.CurHP.ToString() + " / " + player.MaxHP.ToString();
        MP.text = player.CurMana.ToString() + " / " + player.MaxMana.ToString();
        LV.text = "Level : " + player.Level.ToString();
        Gold.text = player.Gold.ToString();
        EXP.text = player.Cur_EXP.ToString() + " / " + player.Max_EXP.ToString();
        Diamond.text = GameManager.Diamond.ToString();
    }

    void AttackCool()
    {
        if(CoolTime <= weapon.Rate)
        {
            CoolTime += Time.deltaTime;
            Image1.fillAmount = CoolTime / weapon.Rate;
        }

    }

    void DashTime()
    {
        if(DashCool <= 7)
        {
            DashCool += Time.deltaTime;
            Image2.fillAmount = DashCool / 7f;
        }
    }

    void SkillCool()
    {
        if (QCool <= skill.QcoolTime)
        {
            QCool += Time.fixedDeltaTime;
            ImageQ.fillAmount = QCool / skill.QcoolTime;
        }
        if (WCool <= skill.WcoolTime)
        {
            WCool += Time.fixedDeltaTime;
            ImageW.fillAmount = WCool / skill.WcoolTime;
        }
        if (ECool <= skill.EcoolTime)
        {
            ECool += Time.fixedDeltaTime;
            ImageE.fillAmount = ECool / skill.EcoolTime;
        }
        if (RCool <= skill.RcoolTime)
        {
            RCool += Time.fixedDeltaTime;
            ImageR.fillAmount = RCool / skill.RcoolTime;
        }
        if (VCool <= skill.VcoolTime)
        {
            VCool += Time.fixedDeltaTime;
            ImageV.fillAmount = VCool / skill.VcoolTime;
        }
    }


}
