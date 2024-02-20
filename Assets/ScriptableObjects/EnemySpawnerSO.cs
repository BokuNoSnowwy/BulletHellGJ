using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class EnemyWave
{
    public float timerSpawn;
    public EnemySpawnSO[] enemyWaveArray;
}

[Serializable]
public class EnemyStream
{
    public float timerEndSpawn;
    public float timerMinSpawn;
    public float timerMaxSpawn;
    
    public GameObject[] enemyTypeArray;
}

[CreateAssetMenu(fileName = "EnemySpawner", menuName = "ScriptableObjects/EnemySpawnerSO", order = 1)]
public class EnemySpawnerSO : ScriptableObject
{
    [SerializeField]
    private EnemyWave[] _wavesArray;
    [SerializeField] 
    private EnemyStream[] _enemyStreamArray;

    public EnemyWave[] WavesArray => _wavesArray;
    public EnemyStream[] EnemyStreamArray => _enemyStreamArray;
}

