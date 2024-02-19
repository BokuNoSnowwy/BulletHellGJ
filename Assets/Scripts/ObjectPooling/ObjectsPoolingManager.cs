using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectsPoolingManager : MonoBehaviour
{

    public static ObjectsPoolingManager Instance;

    [SerializeField] private GameObject playerProjectilePrefab;
    [SerializeField] private GameObject enemyProjectilePrefab;
    [SerializeField] private GameObject enemyPrefab;

    private ObjectPool<GameObject> playerProjectilesPool;
    private ObjectPool<GameObject> enemyProjectilesPool;
    private ObjectPool<GameObject> enemiesPool;

    public ObjectPool<GameObject> PlayerProjectilesPool { get { return playerProjectilesPool; } }
    public ObjectPool<GameObject> EnemyProjectilesPool { get { return enemyProjectilesPool; } }
    public ObjectPool<GameObject> EnemiesPool { get { return enemiesPool; } }

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
    }

    void Start()
    {
        playerProjectilesPool = new ObjectPool<GameObject>(CreatePlayerProjectileObject, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 5000);
        enemyProjectilesPool = new ObjectPool<GameObject>(CreateEnnemyProjectileObject, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 5000);
        enemyProjectilesPool = new ObjectPool<GameObject>(CreateEnemiesObject, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 100, 300);
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
    private GameObject CreateEnemiesObject()
    {
        return CreateObjectFromList(enemyPrefab);
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
}
