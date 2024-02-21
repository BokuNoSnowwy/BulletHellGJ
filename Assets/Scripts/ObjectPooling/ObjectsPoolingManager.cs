using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectsPoolingManager : MonoBehaviour
{

    public static ObjectsPoolingManager Instance;

    [SerializeField] private PlayerProjectile playerProjectilePrefab;
    [SerializeField] private EnemyProjectile enemyProjectilePrefab;
    [SerializeField] private Enemy enemyPrefab;

    private ObjectPool<PlayerProjectile> playerProjectilesPool;
    private ObjectPool<EnemyProjectile> enemyProjectilesPool;
    private ObjectPool<Enemy> enemiesPool;

    public ObjectPool<PlayerProjectile> PlayerProjectilesPool { get { return playerProjectilesPool; } }
    public ObjectPool<EnemyProjectile> EnemyProjectilesPool { get { return enemyProjectilesPool; } }
    public ObjectPool<Enemy> EnemiesPool { get { return enemiesPool; } }

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
        if (playerProjectilePrefab != null) playerProjectilesPool = new ObjectPool<PlayerProjectile>(CreatePlayerProjectileObject, OnTakePlayerProjectileFromPool, OnReturnedPlayerProjectileToPool, OnDestroyPlayerProjectilePoolObject, true, 200);
        if (enemyProjectilePrefab != null) enemyProjectilesPool = new ObjectPool<EnemyProjectile>(CreateEnemyProjectileObject, OnTakeEnemyProjectileFromPool, OnReturnedEnemyProjectileToPool, OnDestroyEnemyProjectilePoolObject, true, 100);
        if (enemyPrefab != null) enemiesPool = new ObjectPool<Enemy>(CreateEnemiesObject, OnTakeEnemyFromPool, OnReturnedEnemyToPool, OnDestroyEnemyPoolObject, true, 100, 300);
    }

    #region ObjectCreation
    private PlayerProjectile CreatePlayerProjectileObject()
    {
        PlayerProjectile newProjectile = Instantiate(playerProjectilePrefab);
        newProjectile.gameObject.SetActive(false);
        return newProjectile;
    }
    
    private EnemyProjectile CreateEnemyProjectileObject()
    {
        EnemyProjectile newProjectile = Instantiate(enemyProjectilePrefab);
        newProjectile.gameObject.SetActive(false);
        return newProjectile;
    }
    
    private Enemy CreateEnemiesObject()
    {
        Enemy newEnemy = Instantiate(enemyPrefab);
        newEnemy.gameObject.SetActive(false);
        return newEnemy;
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
    
    // Player Projectile
    
    void OnReturnedPlayerProjectileToPool(PlayerProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }
    void OnTakePlayerProjectileFromPool(PlayerProjectile projectile)
    {
        //enemy.gameObject.SetActive(true);
        projectile.OnTakenFromPool();
    }
    void OnDestroyPlayerProjectilePoolObject(PlayerProjectile projectile)
    {
        Destroy(projectile.gameObject);
    }
    
    // Enemy Projectile
    
    void OnReturnedEnemyProjectileToPool(EnemyProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }
    void OnTakeEnemyProjectileFromPool(EnemyProjectile projectile)
    {
        //enemy.gameObject.SetActive(true);
        projectile.OnTakenFromPool();
    }
    void OnDestroyEnemyProjectilePoolObject(EnemyProjectile projectile)
    {
        Destroy(projectile.gameObject);
    }

    // Enemy
    void OnReturnedEnemyToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }
    void OnTakeEnemyFromPool(Enemy enemy)
    {
        //enemy.gameObject.SetActive(true);
        enemy.OnTakenFromPool();
    }
    void OnDestroyEnemyPoolObject(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}
