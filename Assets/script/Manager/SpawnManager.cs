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
    public GameObject healthBarPrefab; // 체력바 프리팹
    public Transform canvas; // 캔버스

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
            RemainTime.text = "남은 시간 : " + (int)Timer + "초";
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

            monsterHealthPairs.Add((monster, healthbar)); // 몬스터와 체력 바 쌍을 리스트에 추가
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
                // 오브젝트 풀링에서 몬스터 가져오기
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
                    Debug.LogWarning("몬스터 풀에 충분한 몬스터가 없습니다!");
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

        // 활성화되지 않은 몬스터 중에서 무작위로 선택
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
        // 활성화된 몬스터의 수 계산
        int activeMonsterCount = monsterHealthPairs.Count(pair => pair.monster.activeSelf);

        // RemainEnemy 텍스트 업데이트
        RemainEnemy.text = "남은 수 : " + activeMonsterCount.ToString();
    }

}
