using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Components
    public Rigidbody rigid;
    public Animator animator;
    RaycastHit Hit;



    [SerializeField] SkillUI skillUI;
    [SerializeField] MainUI mainUI;
    [SerializeField] GameObject DashEffect;
    [SerializeField] SpawnManager spawnManager;
    [SerializeField] GameObject LevelUpEffect;
    public GameDataManager game; //테스트용;


    [SerializeField] private int Dmg;
    [SerializeField] private int Def;

    //Functions
    public int Speed;
    [SerializeField] private float DashCoolTime;
    public bool isJump;
    [SerializeField] bool IsDamage;
    public bool move;

    [SerializeField] Coroutine coroutine;

    //Stats
    
    private float maxHP;
    private float curHP;
    private float maxMana;
    private float curMana;
    private int level;
    private float max_exp;
    private float cur_exp;
    



    //Property
    public int DMG
    {
        get => Dmg;
        set
        {
            Dmg = value;
        }
    }

    public int DEF
    {
        get => Def;
        set
        {
            Def = value;
        }
    }

    private int gold;
    public int Gold
    {
        get { return gold; }
        set { gold = value; }
    }

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    public float MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }
    public float CurHP
    {
        get { return curHP; }
        set
        {
            curHP = value;
            if (curHP > maxHP)
                curHP = maxHP;
            else if (curHP <= 0)
            {
                curHP = 0;
                move = true;
                if (move)
                {
                    GameDataManager.Instance.Respawn();
                    spawnManager.Gaming = false;
                    spawnManager.Timer = 120;
                }

            }

        }
    }

    public float MaxMana
    {
        get { return maxMana; }
        set { maxMana = value; }
    }
    public float CurMana
    {
        get { return curMana; }
        set
        {
            curMana = value;
            if (curMana > maxMana)
                curMana = maxMana;
            else if (curMana < 0)
                curMana = 0;

        }
    }
    public float Cur_EXP
    {
        get { return cur_exp; }
        set { cur_exp = value; }
    }

    public float Max_EXP
    {
        get { return max_exp; }
        set { max_exp = value; }
    }
    
    //atk Delay
    [SerializeField] private float Atkdelay;
    [SerializeField] bool AtkReady;
    [SerializeField] Weapon weapon;


    public bool Chain = false;
    public bool Chain2 = false;
    public bool attacking;
    [SerializeField] bool KeyInput = false;
    public bool SkillON = false;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        Stats();
    }
    void Update()
    {
        DashEffect.transform.position = transform.position;
        LevelUpEffect.transform.position = transform.position;

        if (DashCoolTime <= 0)
        {
            DashCoolTime = 0;
        }
        else
        {
            DashCoolTime -= Time.deltaTime;
        }
        Atkdelay += Time.deltaTime;
        AtkReady = weapon.Rate < Atkdelay;

        if (!SkillON)
        {
            if (!attacking)
            {
                Running();
                Jump();
            }
            Attack();
        }
        TestMsg();
        LevelUP();
    }

    void TestMsg()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Cur_EXP += 10;
            GameManager.Diamond++;
            mainUI.EXPslider();
        }
    }

    void FixedUpdate()
    {
        if (!SkillON && !attacking)
        {
            Moving();
        }
        rigid.angularVelocity = Vector3.zero;
    }

    void Stats()
    {
        //Player Stats 관리
        if (!GameManager.SaveON)
        {
            
            maxHP = 100;
            curHP = maxHP;

            maxMana = 100;
            curMana = maxMana;
            Dmg = weapon.WeaponDMG;
            Def = 0;
            Gold = 10000;
            Level = 1;
            max_exp = 100;
            cur_exp = 0;
            Speed = 5;
            
        }

        DashCoolTime = 0f;

        isJump = false;
        attacking = false;
        KeyInput = false;
        SkillON = false;
        move = false;
    }

    void Moving()
    {
        float x = Input.GetAxisRaw("Horizontal") * Speed;
        float z = Input.GetAxisRaw("Vertical") * Speed;

        Vector3 DirMove = (Vector3.right * x + Vector3.forward * z).normalized * Time.fixedDeltaTime;

        rigid.velocity = new Vector3(x, rigid.velocity.y, z);
        animator.SetBool("IsWalk", DirMove != Vector3.zero);

        //회전 관련
        if (DirMove != Vector3.zero)
        {
            Quaternion Rotation = Quaternion.LookRotation(DirMove);
            rigid.rotation = Quaternion.Lerp(rigid.rotation, Rotation, 7 * Time.fixedDeltaTime);
        }
    }
    void Running()
    {
        //대쉬 관련 기능
        if (Input.GetKeyDown(KeyCode.LeftShift) && DashCoolTime <= 0)
        {
            DashEffect.SetActive(true);
            skillUI.DashCool = 0;
            skillUI.Image2.fillAmount = 0;
            DashCoolTime = 7f;
            StartCoroutine(Dash());
            animator.SetBool("IsRun", true);
        }
    }

    IEnumerator Dash()
    {
        Speed = Speed * 2;
        yield return new WaitForSeconds(2f);
        Speed = Speed / 2;
        DashEffect.SetActive(false);
        animator.SetBool("IsRun", false);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            rigid.velocity = Vector3.up * 4.5f;
            isJump = true;
        }

        float Distance = 0.2f;

        Debug.DrawRay(rigid.position, Vector3.down * Distance, Color.blue);

        if (Physics.Raycast(rigid.position, Vector3.down, out Hit, Distance) && rigid.velocity.y < 0)
        {
            isJump = false;
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && AtkReady && !attacking && !isJump)
        {
            skillUI.CoolTime = 0;
            skillUI.Image1.fillAmount = 0;
            attacking = true;
            Atkdelay = 0;
            weapon.Use();
            animator.SetTrigger("IsSwing");
            StartCoroutine(ChainTimer());
            StartCoroutine(AttackingStop(0.75f));
        }
        if (Chain && attacking)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                StopCoroutine(ChainTimer());
                StopCoroutine(coroutine);
                Chain = false;
                KeyInput = true;
                weapon.Use2();
                animator.SetBool("Swing2", true);
                StartCoroutine(AnimatorFalse(2f, "Swing2"));
                StartCoroutine(ChainTimer2());
                StartCoroutine(AttackingStop(0.75f));
            }
        }
        if (Chain2 && attacking)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                StopCoroutine(ChainTimer2());
                StopCoroutine(coroutine);
                Chain2 = false;
                KeyInput = true;
                weapon.Use3();
                animator.SetBool("Swing3", true);
                StartCoroutine(AnimatorFalse(2f, "Swing3"));
                StartCoroutine(Attacking2Stop());
            }
        }
    }

    IEnumerator ChainTimer()
    {
        yield return new WaitForSeconds(0.35f);
        Chain = true;

        yield return new WaitForSeconds(0.25f);
        Chain = false;
    }

    IEnumerator ChainTimer2()
    {
        yield return new WaitForSeconds(0.35f);
        KeyInput = false;
        Chain2 = true;
        yield return new WaitForSeconds(0.25f);
        Chain2 = false;

    }

    IEnumerator AttackingStop(float _wait)
    {
        coroutine = StartCoroutine(AttackingStopCoroutine(_wait));
        yield return coroutine;

    }

    IEnumerator AttackingStopCoroutine(float _wait)
    {
        yield return new WaitForSeconds(_wait);
        if (!KeyInput)
        {
            attacking = false;
        }
    }
    IEnumerator Attacking2Stop()
    {
        yield return new WaitForSeconds(1.3f);
        KeyInput = false;
        attacking = false;
    }

    IEnumerator AnimatorFalse(float duration, string boolName)
    {
        yield return new WaitForSeconds(duration);
        animator.SetBool(boolName, false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyBullet")
        {
            if (!IsDamage)
            {
                EnemyArea enemyBullet = other.GetComponent<EnemyArea>();
                CurHP -= (enemyBullet.damage * GameDataManager.Instance.GameStage) - DEF;

                StartCoroutine(OnDamage());
                mainUI.HPslider();
            }
        }
    }

    IEnumerator OnDamage()
    {
        //피격
        IsDamage = true;
        ColorSet(Color.red);
        yield return new WaitForSeconds(0.4f);
        ColorSet(Color.white);
        IsDamage = false;
    }


    void ColorSet(Color color)
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            foreach (Material material in renderer.materials)
            {
                if (material.shader.name.Contains("UnityChanToonShader/NoOutline/ToonColor_ShadingGradeMap"))
                {
                    material.SetColor("_BaseColor", color);
                }
            }
        }
    }

    void LevelUP()
    {
        if (cur_exp >= max_exp)
        {
            LevelUpEffect.SetActive(true);
            Level++;
            Cur_EXP = 0;
            Max_EXP *= 2;

            MaxHP += 50;
            CurHP = MaxHP;

            MaxMana += 10;
            CurMana = MaxMana;

            DMG += 5;
            DEF += 1;

            mainUI.HPslider();
            mainUI.MPslider();
            mainUI.EXPslider();
            StartCoroutine(EffectOFF());
        }
        
    }

    IEnumerator EffectOFF()
    {
        yield return new WaitForSeconds(1f);
        LevelUpEffect.SetActive(false);
    }
}
