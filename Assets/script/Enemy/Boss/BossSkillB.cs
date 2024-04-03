using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossSkillB : MonoBehaviour
{
    public GameObject Marble;
    [SerializeField] GameObject SmitePos;
    public GameObject MagicAttack;
    public GameObject SmiteEffect;

    public GameObject SmiteRange;

    Animator Animator;

    public Transform SkillPos;
    public float launchForce = 10f;
    Rigidbody rigid;
    Enemy enemy;


    public float targetScale = 31f;
    public float duration = 5f;

    void Start()
    {
        SmiteRange.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0.02f);
        Animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        rigid = Marble.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (enemy.EnemyDead)
        {
            MagicAttack.SetActive(false);
            SmiteEffect.SetActive(false);
            SmitePos.SetActive(false);
            SmiteRange.SetActive(false);
            SmiteRange.transform.localScale = Vector3.one;
            SmiteRange.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0.02f);
        }
    }

    public void Shoot()
    {
        if(!enemy.EnemyDead)
        {
            GameObject marbleInstance = Instantiate(Marble, SkillPos.position, SkillPos.rotation);
            Rigidbody marbleRigidbody = marbleInstance.GetComponent<Rigidbody>();
            if (marbleRigidbody != null)
            {
                marbleRigidbody.velocity = SkillPos.forward * launchForce;
            }
            Animator.SetTrigger("SkillB");
        }

    }
    public void Smite()
    {
        if (!enemy.EnemyDead)
        {
            SmiteEffect.transform.position = new Vector3(gameObject.transform.position.x, SmiteEffect.transform.position.y, gameObject.transform.position.z);
            StartCoroutine(ScaleOverTime());

        }

    }

    public void Magic()
    {
        if (!enemy.EnemyDead)
        {
            MagicAttack.transform.position = gameObject.transform.position;
            MagicAttack.SetActive(true);
            StartCoroutine(MagicOFF());
            Animator.SetTrigger("SkillC");
        }
    }

    IEnumerator AreaOFF()
    {
        yield return new WaitForSeconds(0.2f);
        SmitePos.SetActive(false);
        SmiteEffect.SetActive(false);
        SmiteRange.SetActive(false);
        SmiteRange.transform.localScale = Vector3.one;
        SmiteRange.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0.02f);
    }

    IEnumerator MagicOFF()
    {
        yield return new WaitForSeconds(8f);
        MagicAttack.SetActive(false);
    }

    IEnumerator ScaleOverTime()
    {
        SmiteRange.SetActive(true);
        Vector3 startScale = SmiteRange.transform.localScale;
        Vector3 targetScaleVector = new Vector3(targetScale, targetScale, startScale.z);
        float currentTime = 0f;

        while (currentTime <= duration)
        {
            float t = currentTime / duration;
            SmiteRange.transform.localScale = Vector3.Lerp(startScale, targetScaleVector, t);
            float alpha = Mathf.Lerp(0.02f, 0.3f, t);
            SmiteRange.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, alpha);
            currentTime += Time.deltaTime;
            if (currentTime >= 4.5f)
            {
                SmiteRange.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0.5f);
            }
            yield return null;
        }

        SmitePos.SetActive(true);
        SmiteEffect.SetActive(true);
        Animator.SetTrigger("SkillA");
        SmiteRange.transform.localScale = targetScaleVector;
        StartCoroutine(AreaOFF());
    }

}
