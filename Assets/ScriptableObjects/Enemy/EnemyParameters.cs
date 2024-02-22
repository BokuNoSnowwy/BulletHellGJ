using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyParameters", menuName ="ScriptableObjects/New Enemy...")]
public class EnemyParameters : ScriptableObject
{
    [Header("Stats")]
    public float AttackDamage = 4f;
    public float Speed = 2f;
    public float MaxLife = 10f;

    [Space(10)]

    [Header("Sprite")]
    public Sprite Sprite;
}
