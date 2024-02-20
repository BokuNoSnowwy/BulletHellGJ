using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;
    
    [SerializeField]
    private EnemySpawnerSO _spawnerData;

    // Waves
    private EnemyWave[] _enemyWaveOrderedArray;
    private int _indexSpawnWave;
    private EnemyWave _currentEnemyWave;
    // Stream
    private EnemyStream[] _enemyStreamOrderedArray;
    private int _indexSpawnStream;
    private EnemyStream _currentEnemyStream;
    private float _timerSpawnEnemyStream;


    private void Awake()
    {
        _indexSpawnWave = 0;
        _indexSpawnStream = 0;
        SetupOrderSpawnArray();
        
        _currentEnemyWave = _enemyWaveOrderedArray[_indexSpawnWave];
        _currentEnemyStream = _enemyStreamOrderedArray[_indexSpawnStream];
        _timerSpawnEnemyStream = Random.Range(_currentEnemyStream.timerMinSpawn, _currentEnemyStream.timerMaxSpawn);
    }

    void Update()
    {
        if (_gameManager.GameState == GameState.Game)
        {
            #region Waves Spawn
            if (_currentEnemyWave.timerSpawn <= _gameManager.TimerGame)
            {
                // Spawn Enemies Waves
                foreach (var enemy in _currentEnemyWave.enemyWaveArray)
                {
                    Debug.Log("Spawn Wave " + enemy.EnemyPrefab + " to " + enemy.SideSpawn + " perecentage : " + enemy.PercentageSideSpawn);
                }
            
                // Increment index spawner
                _indexSpawnWave++;
                if (_indexSpawnWave < _enemyWaveOrderedArray.Length)
                {
                    _currentEnemyWave = _enemyWaveOrderedArray[_indexSpawnWave];
                }
                else
                {
                    Debug.Log("Spawn Boss");
                }
            }
            #endregion
                    
                      
            #region Stream Spawn
            
            if (_currentEnemyStream.timerEndSpawn <= _gameManager.TimerGame)
            {
                _indexSpawnStream++;
                if (_indexSpawnStream < _enemyStreamOrderedArray.Length)
                {
                    _currentEnemyStream = _enemyStreamOrderedArray[_indexSpawnStream];
                    _timerSpawnEnemyStream = Random.Range(_currentEnemyStream.timerMinSpawn, _currentEnemyStream.timerMaxSpawn);
                }
            }
            else
            {
                _timerSpawnEnemyStream -= Time.deltaTime;
                if (_timerSpawnEnemyStream <= 0)
                {
                    _timerSpawnEnemyStream = Random.Range(_currentEnemyStream.timerMinSpawn, _currentEnemyStream.timerMaxSpawn);
                    //Spawn Enemies Stream 
                    SpawnRandom(_currentEnemyStream.enemyTypeArray);
                }
            }
            #endregion
        }
    }

    private void SetupOrderSpawnArray()
    {
        _enemyWaveOrderedArray = _spawnerData.WavesArray.OrderBy(spawn => spawn.timerSpawn).ToArray();
        _enemyStreamOrderedArray = _spawnerData.EnemyStreamArray.OrderBy(stream => stream.timerEndSpawn).ToArray();
    }

    private void SpawnRandom(GameObject[] enemyArray)
    {
        GameObject randomEnemy = enemyArray[Random.Range(0, enemyArray.Length)];
        CameraSide randomSide = (CameraSide) Random.Range(0, 4);
        float randomPercentageSide = Random.Range(0, 101);
        
        // TODO Spawn Enemy
        Debug.Log("Spawn Prefab " + randomEnemy + " to " + randomSide + " percentage : " + randomPercentageSide);
    }

}