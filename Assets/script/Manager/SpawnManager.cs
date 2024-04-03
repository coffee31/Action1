using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] Prefab;
    [SerializeField] List<Transform> spawnPoint;

    [SerializeField] GameObject RemainPanel;
    [SerializeField] TMP_Text RemainEnemy;
    [SerializeField] TMP_Text RemainTime;

    [SerializeField] GameObject FieldInfo;
    bool InfoON;

    [SerializeField] Transform spawntransform;
    public float Timer;
    public bool Gaming = false;

    float Second;

    List<(GameObject monster, GameObject healthbar)> monsterHealthPairs = new List<(GameObject, GameObject)>();
    public GameObject healthBarPrefab; // ü�¹� ������
    public Transform canvas; // ĵ����

    void Start()
    {
        Second = 5f;
        Timer = 120;
        Gaming = false;
        InfoON = true;
        spawntransform = spawnPoint[Random.Range(0, spawnPoint.Count)];
        MonsterCreate();
    }
    private void Update()
    {
        if (GameDataManager.Instance.GameStart)
        {
            Gaming = true;
            StartCoroutine(SpawnMonster());
        }

        if(Gaming)
        {
            RemainPanel.SetActive(true);
            Timer -= Time.deltaTime;
            RemainTime.text = "���� �ð� : " + (int)Timer + "��";
            RemainEnemyCount();
            if (Timer <= 0)
            {
                
                Timer = 0;
                GameDataManager.Instance.Respawn();
                Gaming = false;
                
            }
        } 
        if(!Gaming)
        {
            FalseMonstersAndHealthBars();
            RemainPanel.SetActive(false);
            Timer = 120;
            InfoON = true;
        }

        if(Timer <= 59)
        {
            if (InfoON)
            {
                FieldInfo.SetActive(true);
                StartCoroutine(InfoBoolOFF());
            }
            MonsterCheck();
        }
    }

    void MonsterCreate()
    {
        for(int i = 0; i < 30; i++)
        {
            GameObject monster = Instantiate(Prefab[Random.Range(0,Prefab.Length)],spawntransform.position , Quaternion.identity);
            monster.SetActive(false);

            GameObject healthbar = Instantiate(healthBarPrefab);
            healthbar.transform.SetParent(canvas.transform);
            healthbar.SetActive(false);
            healthbar.GetComponent<EnemyBar>().Setup(monster);

            monsterHealthPairs.Add((monster, healthbar)); // ���Ϳ� ü�� �� ���� ����Ʈ�� �߰�
        }
    }


    IEnumerator SpawnMonster()
    {
        while(Timer > 59 && Gaming)
        {
            GameDataManager.Instance.GameStart = false;
            int RandomPoint = Random.Range(0, spawnPoint.Count);
            spawntransform = spawnPoint[RandomPoint];

            if (spawntransform != null)
            {
                // ������Ʈ Ǯ������ ���� ��������
                GameObject monster = GetPooledMonster();
                if (monster != null)
                {
                    monster.transform.position = spawntransform.position;
                    monster.SetActive(true);

                    foreach (var pair in monsterHealthPairs)
                    {
                        if (pair.monster == monster)
                        {
                            pair.healthbar.SetActive(true);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("���� Ǯ�� ����� ���Ͱ� �����ϴ�!");
                }
            }
            yield return new WaitForSeconds(Second);
        }

    }

    GameObject GetPooledMonster()
    {
        List<GameObject> inactiveMonsters = new List<GameObject>();
        foreach (var pair in monsterHealthPairs)
        {
            if (!pair.monster.activeInHierarchy)
            {
                inactiveMonsters.Add(pair.monster);
            }
        }

        // Ȱ��ȭ���� ���� ���� �߿��� �������� ����
        if (inactiveMonsters.Count > 0)
        {
            return inactiveMonsters[Random.Range(0, inactiveMonsters.Count)];
        }
        return null;
    }

    void FalseMonstersAndHealthBars()
    {
        foreach (var pair in monsterHealthPairs)
        {
            pair.monster.SetActive(false);
            pair.healthbar.SetActive(false);
        }
    }

    void MonsterCheck()
    {
        bool allActiveEnemy = true;

        foreach (var pair in monsterHealthPairs)
        {
            if (pair.monster.activeSelf)
            {
                allActiveEnemy = false;
                break;
            }
        }

        if (allActiveEnemy)
        {
            GameDataManager.Instance.Respawn();
            Gaming = false;
        }
    }

    IEnumerator InfoBoolOFF()
    {
        InfoON = false;
        yield return new WaitForSeconds(3);
        FieldInfo.SetActive(false);

    }

    void RemainEnemyCount()
    {
        // Ȱ��ȭ�� ������ �� ���
        int activeMonsterCount = monsterHealthPairs.Count(pair => pair.monster.activeSelf);

        // RemainEnemy �ؽ�Ʈ ������Ʈ
        RemainEnemy.text = "���� �� : " + activeMonsterCount.ToString();
    }

}
