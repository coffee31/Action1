using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]private enum Type { Melee, Range };
    private Type type;
    private int WeaponDmg;

    public int WeaponDMG
    {
        get => WeaponDmg;
    }
    private float rate;
    public float Rate
    {
        get => rate;
    }

    [SerializeField] BoxCollider Area;
    [SerializeField] TrailRenderer Trail;
    [SerializeField] Player player;


    //데미지는 플레이어 공격력을 받아오도록함.
    void Awake()
    {
        WeaponDmg = 5;
    }

    void Start()
    {
        rate = 0.6f;
    }

    public void Use()
    {
        if (type == Type.Melee)
        {
            StartCoroutine(SwingCoroutine(0.35f, 0.2f));
        }
    }
    
    public void Use2()
    {
        if (type == Type.Melee)
        {
            StartCoroutine(ChainBonus((int)(player.DMG * 0.5f),0.15f,0.6f));
            StartCoroutine(SwingCoroutine(0.37f, 0.3f));
        }
    }
    public void Use3()
    {
        if (type == Type.Melee)
        {
            StartCoroutine(ChainBonus((int)(player.DMG * 0.5f), 0.3f,1f));
            StartCoroutine(SwingCoroutine(0.8f, 0f));
        }
    }
    IEnumerator SwingCoroutine(float delay1, float delay2)
    {
        yield return new WaitForSeconds(delay1);
        Area.enabled = true;
        Trail.enabled = true;
        yield return new WaitForSeconds(0.25f);
        Trail.enabled = false;
        yield return new WaitForSeconds(0.5f);
        Area.enabled = false;

    }
    
    IEnumerator ChainBonus(int Bonus, float Wait, float Wait2)
    {
        yield return new WaitForSeconds(Wait);
        player.BonusDMG += Bonus;
        yield return new WaitForSeconds(Wait2);
        player.BonusDMG -= Bonus;
    }

}
