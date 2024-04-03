using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArea : MonoBehaviour
{
    [SerializeField] private int Damage;
    [SerializeField] Enemy enemy;

    public int damage
    {
        get => Damage;
        set { Damage = value; }
    }

    private void Awake()
    {
        if (enemy.EnemyType == Enemy.Type.A)
            damage = 5;
        else if (enemy.EnemyType == Enemy.Type.B)
            damage = 8;
        else if (enemy.EnemyType == Enemy.Type.C)
            damage = 50;
    }
}
