using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectsPoolingManager : MonoBehaviour
{

    public static ObjectsPoolingManager Instance;

    [SerializeField] private GameObject playerProjectilePrefab;
    [SerializeField] private GameObject enemyProjectilePrefab;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private ExperiencePoint expPrefab;

    private ObjectPool<GameObject> playerProjectilesPool;
    private ObjectPool<GameObject> enemyProjectilesPool;
    private ObjectPool<Enemy> enemiesPool;
    private ObjectPool<ExperiencePoint> expPool;

    public ObjectPool<GameObject> PlayerProjectilesPool { get { return playerProjectilesPool; } }
    public ObjectPool<GameObject> EnemyProjectilesPool { get { return enemyProjectilesPool; } }
    public ObjectPool<Enemy> EnemiesPool { get { return enemiesPool; } }
    public ObjectPool<ExperiencePoint> ExpPool { get { return expPool; } }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        if (playerProjectilePrefab != null) playerProjectilesPool = new ObjectPool<GameObject>(CreatePlayerProjectileObject, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 5000);
        if (enemyProjectilePrefab != null) enemyProjectilesPool = new ObjectPool<GameObject>(CreateEnnemyProjectileObject, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 5000);
        if (enemyPrefab != null) enemiesPool = new ObjectPool<Enemy>(CreateEnemiesObject, OnTakeEnemyFromPool, OnReturnedEnemyToPool, OnDestroyEnemyPoolObject, true, 100, 300);
        if (expPrefab != null) expPool = new ObjectPool<ExperiencePoint>(CreateExpObject, OnTakeExpFromPool, OnReturnedExpToPool, OnDestroyExpPoolObject, true, 100, 300);
    }

    #region ObjectCreation
    private GameObject CreatePlayerProjectileObject()
    {
        return CreateObjectFromList(playerProjectilePrefab);
    }
    private GameObject CreateEnnemyProjectileObject()
    {
        return CreateObjectFromList(enemyProjectilePrefab);
    }
    private Enemy CreateEnemiesObject()
    {
        Enemy newEnemy = Instantiate(enemyPrefab);
        newEnemy.gameObject.SetActive(false);
        return newEnemy;
    }
    private ExperiencePoint CreateExpObject()
    {
        ExperiencePoint newExp = Instantiate(expPrefab);
        newExp.gameObject.SetActive(false);
        return newExp;
    }

    private GameObject CreateObjectFromList(GameObject gameObject)
    {
        GameObject newObj = Instantiate(gameObject);
        newObj.SetActive(false);
        return newObj;
    }
    #endregion
    void OnReturnedToPool(GameObject gObject)
    {
        gObject.SetActive(false);
    }
    void OnTakeFromPool(GameObject gObject)
    {
        gObject.SetActive(true);
    }
    void OnDestroyPoolObject(GameObject gObject)
    {
        Destroy(gObject);
    }

    void OnReturnedEnemyToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }
    void OnReturnedExpToPool(ExperiencePoint exp)
    {
        exp.gameObject.SetActive(false);
    }
    void OnTakeEnemyFromPool(Enemy enemy)
    {
        //enemy.gameObject.SetActive(true);
        enemy.OnTakenFromPool();
    }
    void OnTakeExpFromPool(ExperiencePoint exp)
    {
        //enemy.gameObject.SetActive(true);
        exp.OnTakenFromPool();
    }
    void OnDestroyEnemyPoolObject(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }void OnDestroyExpPoolObject(ExperiencePoint exp)
    {
        Destroy(exp.gameObject);
    }
}
