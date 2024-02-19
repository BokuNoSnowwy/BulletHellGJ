using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraSide
{
    Top,
    Bottom,
    Left,
    Right
}

[CreateAssetMenu(fileName = "EnemySpawn", menuName = "ScriptableObjects/EnemySpawnSO", order = 1)]
public class EnemySpawnSO : ScriptableObject
{
    [SerializeField]
    private CameraSide _sideSpawn;
    [SerializeField]
    [Range(0, 100)] private float _percentageSideSpawn;
    [SerializeField]
    private GameObject _enemyPrefab;

    public CameraSide SideSpawn => _sideSpawn;

    public float PercentageSideSpawn => _percentageSideSpawn;

    public GameObject EnemyPrefab => _enemyPrefab;
}
