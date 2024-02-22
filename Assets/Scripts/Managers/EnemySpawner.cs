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

    [SerializeField]
    private Camera _camera;

    // Waves
    private EnemyWave[] _enemyWaveOrderedArray;
    private int _indexSpawnWave;
    private EnemyWave _currentEnemyWave;
    // Stream
    private EnemyStream[] _enemyStreamOrderedArray;
    private int _indexSpawnStream;
    private EnemyStream _currentEnemyStream;
    private float _timerSpawnEnemyStream;

    private const float POS_MAX_OFFSET = 1.05f;
    private const float POS_MIN_OFFSET = -0.05f;

    private bool _waveDone = false;

    private float _streamTimer = 0f;

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
         
            if (_waveDone == false && _currentEnemyWave.timerSpawn <= _gameManager.TimerGame)
            {
                // Spawn Enemies Waves
                for (int i = 0; i < _currentEnemyWave.NumberOfEnemies; i++)
                {
                    SpawnOnSide(_currentEnemyWave.enemyWaveArray.EnemyPrefab, _currentEnemyWave.enemyWaveArray.SideSpawn);
                }

                // Increment index spawner
                _indexSpawnWave++;
                if (_indexSpawnWave < _enemyWaveOrderedArray.Length)
                {
                    _currentEnemyWave = _enemyWaveOrderedArray[_indexSpawnWave];
                }
                else
                {
                    _waveDone = true;
                }
            }
           
            #endregion
                    
                      
            #region Stream Spawn
            if(_streamTimer < _currentEnemyStream.timerEndSpawn)
            {
                _streamTimer += Time.deltaTime;
            }
            else
            {
                _streamTimer = 0f;
                SetStreamIndex();
            }

            if(_timerSpawnEnemyStream <= 0f)
            {
                _timerSpawnEnemyStream = Random.Range(_currentEnemyStream.timerMinSpawn, _currentEnemyStream.timerMaxSpawn); 
                SpawnRandom(_currentEnemyStream.enemyTypeArray);
            }
            else
            {
                _timerSpawnEnemyStream -= Time.deltaTime;
            }

            #endregion
        }
    }

    private void SetStreamIndex()
    {
        _indexSpawnStream++;
        if (_indexSpawnStream < _enemyStreamOrderedArray.Length)
        {
            _currentEnemyStream = _enemyStreamOrderedArray[_indexSpawnStream];
            _timerSpawnEnemyStream = Random.Range(_currentEnemyStream.timerMinSpawn, _currentEnemyStream.timerMaxSpawn);
        }
        else
        {
            _indexSpawnStream = 0;
        }
    }

    private void SetupOrderSpawnArray()
    {
        _enemyWaveOrderedArray = _spawnerData.WavesArray.OrderBy(spawn => spawn.timerSpawn).ToArray();
        _enemyStreamOrderedArray = _spawnerData.EnemyStreamArray.OrderBy(stream => stream.timerEndSpawn).ToArray();
    }

    private void SpawnRandom(EnemyParameters[] enemyArray)
    {
        EnemyParameters randomEnemy = enemyArray[Random.Range(0, enemyArray.Length)];
        CameraSide randomSide = (CameraSide) Random.Range(0, 4);
        float randomPercentageSide = Random.Range(0, 101);
        
        Enemy enemy = ObjectsPoolingManager.Instance.EnemiesPool.Get();
        Vector3 enemySpawnPosition = new Vector3(0,0, _camera.nearClipPlane);
        switch (randomSide)
        {
            case CameraSide.Top:
                enemySpawnPosition.x = (randomPercentageSide / 100);
                enemySpawnPosition.y = POS_MAX_OFFSET;
                break;
            case CameraSide.Bottom:
                enemySpawnPosition.x = (randomPercentageSide / 100);
                enemySpawnPosition.y = POS_MIN_OFFSET;
                break;
            case CameraSide.Left:
                enemySpawnPosition.x = POS_MIN_OFFSET;
                enemySpawnPosition.y = (randomPercentageSide / 100);
                break;
            case CameraSide.Right:
                enemySpawnPosition.x = POS_MAX_OFFSET;
                enemySpawnPosition.y = (randomPercentageSide / 100);
                break;
        }
        enemy.gameObject.transform.position = _camera.ViewportToWorldPoint(enemySpawnPosition);

        enemy.Initialization(randomEnemy);
    }

    private void SpawnOnSide(EnemyParameters parameters, CameraSide side)
    {
        Enemy enemy = ObjectsPoolingManager.Instance.EnemiesPool.Get();
        float randomPercentageSide = Random.Range(0, 101);
        Vector3 enemySpawnPosition = new Vector3(0, 0, _camera.nearClipPlane);

        switch (side)
        {
            case CameraSide.Top:
                enemySpawnPosition.x = (randomPercentageSide / 100);
                enemySpawnPosition.y = POS_MAX_OFFSET;
                break;
            case CameraSide.Bottom:
                enemySpawnPosition.x = (randomPercentageSide / 100);
                enemySpawnPosition.y = POS_MIN_OFFSET;
                break;
            case CameraSide.Left:
                enemySpawnPosition.x = POS_MIN_OFFSET;
                enemySpawnPosition.y = (randomPercentageSide / 100);
                break;
            case CameraSide.Right:
                enemySpawnPosition.x = POS_MAX_OFFSET;
                enemySpawnPosition.y = (randomPercentageSide / 100);
                break;
        }
        enemy.gameObject.transform.position = _camera.ViewportToWorldPoint(enemySpawnPosition);

        enemy.Initialization(parameters);
    }

}