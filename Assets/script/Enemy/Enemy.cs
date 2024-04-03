using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public enum EnemyState // State 패턴
{
    Idle,   
    Chasing, 
    Attacking 
}
public class Enemy : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] MainUI mainUI;
    Material Mats;
    Material mat;
    NavMeshAgent nav;
    BossSkillB bossSkill;
    Animator ani;
    RaycastHit[] rayHits;

    public enum Type { A, B, C };
    public Type EnemyType;
    [SerializeField] EnemyState currentState = EnemyState.Idle;

    //Enmey Stats
    [SerializeField] private float maxHP;
    [SerializeField] private float curHP;


    [SerializeField] int RandomNum;
    public bool EnemyDead;
    public float CurHP
    {
        get { return curHP; }
        set
        {
            curHP = value;
            if (curHP >= maxHP)
                curHP = maxHP;
            else if (curHP <= 0)
                curHP = 0;
        }
    }

    public float MaxHP
    {
        get { return maxHP; }
    }
    [SerializeField] private bool AttackON;

    //Target
    [SerializeField] Player player;
    [SerializeField] Transform Target;
    [SerializeField] private bool Chase;
    float targetRadius = 0;
    float targetRange = 0;
    Vector3 ReturnPosition;

    //Attack
    [SerializeField] BoxCollider meleeArea;
    [SerializeField] bool DamageON;
    [SerializeField] bool Attacking;
    [SerializeField] bool Patton;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        bossSkill = GetComponent<BossSkillB>();
        sphereCollider = GetComponent<SphereCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        mainUI = GameObject.FindWithTag("MainUI").GetComponent<MainUI>();
        ani = GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
        if (EnemyType != Type.A)
            Mats = GetComponentInChildren<MeshRenderer>().material;
    }

    void Start()
    {
        Initialized();
        nav.updateRotation = false;
    }

    void Initialized()
    {
        Stats();
        Target = null;
        EnemyDead = false;
    }

    void Stats()
    {
        switch (EnemyType)
        {
            case Type.A:
                maxHP = 30 * GameDataManager.Instance.GameStage;
                targetRadius = 10f;
                targetRange = 2f;
                break;
            case Type.B:
                maxHP = 50 * GameDataManager.Instance.GameStage;
                targetRadius = 10f;
                targetRange = 4f;
                break;
            case Type.C:
                maxHP = 1500 * GameDataManager.Instance.GameStage;
                targetRadius = 12f;
                targetRange = 8f;
                break;
        }
        curHP = maxHP;

    }

    void IdleState()
    {
        if (EnemyType != Type.C)
        {
            AttackON = false;
            Chase = false;
            Attacking = false;
            ani.SetBool("IsWalk", false);
            ani.SetBool("IsAtk", false);
        }
        else
        {
            AttackON = false;
            Chase = false;
            Attacking = false;
        }
    }
    void ChaseState()
    {
        if (EnemyType != Type.C)
        {
            Chase = true;
            ani.SetBool("IsWalk", true);
            ani.SetBool("IsAtk", false);
        }
        else
        {
            Chase = true;
        }
    }
    void AttackState()
    {
        if (EnemyType != Type.C)
        {
            Chase = false;
            AttackON = true;
            StartCoroutine(Attack());
            ani.SetBool("IsAtk", true);
            ani.SetBool("IsWalk", true);
        }
        else
        {
            Chase = false;
            AttackON = true;
            StartCoroutine(Attack());
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                IdleState();
                break;
            case EnemyState.Chasing:
                ChaseState();
                break;
            case EnemyState.Attacking:
                AttackState();
                break;
        }
    }

    void FixedUpdate()
    {
        if (currentState != EnemyState.Attacking)
        {
            rigid.velocity = Vector3.zero;
        }

        TargetingCheck();
        if (nav.enabled)
        {
            nav.isStopped = !Chase;
            if (Target != null)
            {
                nav.SetDestination(Target.position);

                Vector3 direction = Target.position - transform.position;
                direction.y = 0f; // 수직 축 회전을 막음
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * nav.angularSpeed);
            }
        }
    }

    void OnEnable()
    {
        Attacking = false;
        Patton = false;
        EnemyDead = false;
        AttackON = false;
        DamageON = false;
        currentState = EnemyState.Idle;
        CurHP = maxHP;
        mat.color = Color.white;
        if (EnemyType == Type.B || EnemyType == Type.C)
            Mats.color = Color.white;
        ReturnPosition = transform.position;

    }


    void TargetingCheck()
    {
        float BackDistance = Vector3.Distance(transform.position, ReturnPosition);
        rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0)
        {
            Target = GameObject.FindWithTag("Player").GetComponent<Transform>();
            float Distance = Vector3.Distance(transform.position, Target.position);

            if (currentState == EnemyState.Idle && Distance <= targetRadius)
            {
                currentState = EnemyState.Chasing;
            }
            else if (currentState == EnemyState.Chasing && Distance <= targetRange)
            {
                currentState = EnemyState.Attacking;
            }
            else if (currentState == EnemyState.Attacking && Distance > targetRange)
            {
                if (EnemyType == Type.C)
                {
                    if (!Patton)
                    {
                        StopCoroutine(Attack());
                        StopCoroutine(BossPatton());
                        currentState = EnemyState.Chasing;
                    }
                }
                else
                {
                    StopCoroutine(Attack());
                    currentState = EnemyState.Chasing;
                }
            }
        }
        else
        {
            if (currentState != EnemyState.Idle)
            {
                if (EnemyType == Type.C)
                {
                    if (!Patton)
                    {
                        Target = null;
                        if (nav.enabled)
                            nav.SetDestination(ReturnPosition);
                        currentState = EnemyState.Chasing;
                        Quaternion targetRotation = Quaternion.LookRotation(ReturnPosition);
                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * nav.angularSpeed);
                        if (BackDistance < 1.0f)
                            currentState = EnemyState.Idle;
                    }

                }
                else
                {
                    Target = null;
                    if (nav.enabled)
                        nav.SetDestination(ReturnPosition);
                    currentState = EnemyState.Chasing;
                    Quaternion targetRotation = Quaternion.LookRotation(ReturnPosition);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * nav.angularSpeed);
                    if (BackDistance < 1.0f)
                        currentState = EnemyState.Idle;
                }


            }
        }
    }
    IEnumerator Attack()
    {
        if (!Attacking && !EnemyDead)
        {
            Attacking = true;
            switch (EnemyType)
            {
                case Type.A:
                    yield return new WaitForSeconds(0.3f);
                    meleeArea.enabled = true;

                    yield return new WaitForSeconds(0.4f);
                    rigid.angularVelocity = Vector3.zero;
                    rigid.velocity = Vector3.zero;
                    meleeArea.enabled = false;


                    yield return new WaitForSeconds(0.6f);
                    Attacking = false;
                    break;
                case Type.B:
                    yield return new WaitForSeconds(0.3f);
                    rigid.AddForce(transform.forward * 10, ForceMode.Impulse);
                    meleeArea.enabled = true;


                    yield return new WaitForSeconds(0.4f);
                    rigid.velocity = Vector3.zero;
                    rigid.angularVelocity = Vector3.zero;
                    meleeArea.enabled = false;

                    Vector3 recoilDirection = -transform.forward * 1.5f; // 반동 방향
                    recoilDirection.y = 0f; // 수직 방향으로는 밀리지 않도록 설정
                    rigid.AddForce(recoilDirection, ForceMode.Impulse); // 반동 힘을 가함

                    yield return new WaitForSeconds(1f);
                    Attacking = false;
                    break;
                case Type.C:
                    StartCoroutine(BossPatton());
                    break;
            }
        }
    }

    IEnumerator BossPatton()
    {
        Attacking = true;
        yield return null;
        RandomNum = Random.Range(0, 6);
        switch (RandomNum)
        {
            case 0:
                StartCoroutine(ShootPatton());
                break;
            case 1:
                StartCoroutine(SmitePatton());
                break;
            case 2:
                StartCoroutine(MagicPatton());
                break;
            case 3:
                StartCoroutine(ShootPatton());
                break;
            case 4:
                StartCoroutine(SmitePatton());
                break;
            case 5:
                StartCoroutine(MagicPatton());
                break;
        }
    }

    IEnumerator ShootPatton()
    {
        Patton = true;
        bossSkill.Shoot();
        yield return new WaitForSeconds(3.0f);
        bossSkill.Shoot();
        yield return new WaitForSeconds(3.0f);
        bossSkill.Shoot();
        yield return new WaitForSeconds(2.0f);
        Attacking = false;
        if (Target != null)
        {
            float Distance = Vector3.Distance(transform.position, Target.position);
            if (Distance > targetRange)
            {
                Patton = false;
                currentState = EnemyState.Chasing;
            }
        }
        if (Patton)
            StartCoroutine(BossPatton());
    }

    IEnumerator SmitePatton()
    {
        Patton = true;
        bossSkill.Smite();
        yield return new WaitForSeconds(7.0f);
        Attacking = false;
        if (Target != null)
        {
            float Distance = Vector3.Distance(transform.position, Target.position);
            if (Distance > targetRange)
            {
                Patton = false;
                currentState = EnemyState.Chasing;
            }
        }
        if (Patton)
            StartCoroutine(BossPatton());
    }

    IEnumerator MagicPatton()
    {
        Patton = true;
        yield return new WaitForSeconds(2.0f);
        bossSkill.Magic();
        yield return new WaitForSeconds(10.0f);
        Attacking = false;
        if (Target != null)
        {
            float Distance = Vector3.Distance(transform.position, Target.position);
            if (Distance > targetRange)
            {
                Patton = false;
                currentState = EnemyState.Chasing;
            }
        }
        if (Patton)
            StartCoroutine(BossPatton());
    }


    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Melee" && !DamageON && gameObject.layer == 7)
        {
            CurHP -= player.DMG;
            Vector3 KnockBack = transform.position - other.transform.position;
            StartCoroutine(Damage(KnockBack));
        }
        if (other.tag == "AreaQ" && !DamageON && gameObject.layer == 7)
        {
            CurHP -= (int)(player.DMG * 1.5f);

            Vector3 KnockBack = transform.position - other.transform.position;
            StartCoroutine(Damage(KnockBack));
        }
        if (other.tag == "AreaW" && !DamageON && gameObject.layer == 7)
        {
            CurHP -= (int)(player.DMG * 3f);

            Vector3 KnockBack = transform.position - other.transform.position;
            StartCoroutine(Damage(KnockBack));
        }
        if (other.tag == "AreaR" && !DamageON && gameObject.layer == 7)
        {
            CurHP -= (int)(player.DMG * 5f);

            Vector3 KnockBack = transform.position - other.transform.position;
            StartCoroutine(Damage(KnockBack));
        }
    }
    IEnumerator Damage(Vector3 KnockBack)
    {
        if (curHP > 0)
        {
            mat.color = Color.red;

            if (EnemyType == Type.B)
            {
                Mats.color = Color.red;
            }

            if (EnemyType == Type.C)
            {
                DamageON = true;
                yield return new WaitForSeconds(0.4f);
                DamageON = false;
            }
            else
            {
                DamageON = true;
                yield return new WaitForSeconds(0.25f);
                DamageON = false;
            }
            mat.color = Color.white;
            if (EnemyType == Type.B || EnemyType == Type.C)
                Mats.color = Color.white;
        }
        else
        {
            gameObject.layer = 8;
            EnemyDead = true;

            if (EnemyType == Type.C)
            {
                StopCoroutine(MagicPatton());
                StopCoroutine(SmitePatton());
                StopCoroutine(ShootPatton());
                StopCoroutine(BossPatton());
                GameDataManager.Instance.BossCount++;
            }
            EXPUP();
            mat.color = Color.gray;
            if (EnemyType == Type.B || EnemyType == Type.C)
                Mats.color = Color.gray;

            Chase = false;
            nav.enabled = false;
            ani.SetTrigger("Die");
            KnockBack = KnockBack.normalized;
            KnockBack += Vector3.up;
            rigid.AddForce(KnockBack * 1, ForceMode.Impulse);


            yield return new WaitForSeconds(2.0f);
            Attacking = false;
            Patton = false;
            EnemyDead = false;
            AttackON = false;
            currentState = EnemyState.Idle;
            gameObject.SetActive(false);
            gameObject.layer = 7;
            nav.enabled = true;
            CurHP = maxHP;
            mat.color = Color.white;
            if (EnemyType == Type.B || EnemyType == Type.C)
                Mats.color = Color.white;

            if (EnemyType == Type.C)
            {
                GameDataManager.Instance.GameStage++;
                GameDataManager.Instance.Respawn();
            }
                



        }

        void EXPUP()
        {
            if (EnemyType == Type.A)
            {
                player.Cur_EXP += 10 * GameDataManager.Instance.GameStage;
                player.Gold += 30 * GameDataManager.Instance.GameStage;
            }
            else if (EnemyType == Type.B)
            {
                player.Cur_EXP += 20 * GameDataManager.Instance.GameStage;
                player.Gold += 50 * GameDataManager.Instance.GameStage;
            }
            else if (EnemyType == Type.C)
            {
                player.Cur_EXP += 500 * GameDataManager.Instance.GameStage;
                player.Gold += 1000 * GameDataManager.Instance.GameStage;
            }

            mainUI.EXPslider();
        }
    }
}
