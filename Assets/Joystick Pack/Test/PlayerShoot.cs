using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerShoot : MonoBehaviour
{
    //new Weapon system
    [SerializeField]
    private Transform shootingTransform;

    private PlayerInventory _playerInventory;
    private ObjectsPoolingManager _poolingManager;
    private GameState _gameState;
    private GameManager _gameManager;

    void Start()
    {

        _gameManager = GameManager.Instance;
        _gameState = _gameManager.GameState;
        _gameManager.AddGameStateChangeListener(ChangeGameState);
        
        _poolingManager = ObjectsPoolingManager.Instance;
        _playerInventory = GetComponent<PlayerInventory>();
        
    }

    void Update()
    {
        //nearest enemy 
        if (_gameState == GameState.Pause)
        {
            return;
        }
        
        foreach (var weapon in _playerInventory.PlayerWeaponArray)
        {
            if (weapon.weaponPlayerSo != null)
            {
                weapon.timerShoot -= Time.deltaTime;
                if (weapon.timerShoot <= 0)
                {
                    int projectileNb = weapon.GetTotalProjectiles();
                    float projectileDmg = weapon.GetTotalProjectileDamage() * _playerInventory.GetTotalPassiveProjectileDamageMultiplier();
                    float projectileSpd = weapon.GetTotalProjectileSpd() * _playerInventory.GetTotalPassiveProjectileSpeed();
                    float projectileScale = weapon.GetTotalProjectileScale() * _playerInventory.GetTotalPassiveProjectileScale();
                
                    switch (weapon.weaponPlayerSo.projectileType)
                    {
                        case WeaponProjectileType.EveryDirection :
                            //TODO Get the number of projectile and divide it from 360 degres 
                            for (int i = 0; i < projectileNb; i++)
                            {
                                PlayerProjectile playerProjectile = _poolingManager.PlayerProjectilesPool.Get();
                                playerProjectile.transform.position = transform.position;
                                float angle = (360f / projectileNb) * i;
                                playerProjectile.transform.rotation = Quaternion.Euler(0,0,angle);
                                playerProjectile.SetupProjectile(projectileDmg, projectileSpd, projectileScale);
                            }
                            break;
                        case WeaponProjectileType.FollowPlayerDirection : 
                            //TODO Multiple projectiles in a row (intern timers)
                            for (int i = 0; i < projectileNb; i++)
                            {
                                StartCoroutine(ShootAfterTime(weapon.GetTotalTimerBetweenShoot() * i, () =>
                                {
                                    PlayerProjectile playerProjectile = _poolingManager.PlayerProjectilesPool.Get();
                                    playerProjectile.transform.position = transform.position;
                                    playerProjectile.transform.up = -shootingTransform.right;
                                    playerProjectile.SetupProjectile(projectileDmg, projectileSpd, projectileScale);
                                }));
                            } 
                            break;
                        case WeaponProjectileType.TargetAtNearestEnemy : 
                            //TODO Time between shoots not working properly 
                            for (int i = 0; i < projectileNb; i++)
                            {
                                StartCoroutine(ShootAfterTime(weapon.GetTotalTimerBetweenShoot() * i, () =>
                                {
                                    Vector3 nearestEnemyPos = GetNearestEnemyPos();
                                    if (nearestEnemyPos != Vector3.zero)
                                    {
                                        PlayerProjectile playerProjectile = _poolingManager.PlayerProjectilesPool.Get();
                                        playerProjectile.transform.position = transform.position;
                                        float angle = PointRightAtTarget(nearestEnemyPos);
                                        playerProjectile.transform.rotation = Quaternion.Euler(0,0,angle);
                                        playerProjectile.SetupProjectile(projectileDmg, projectileSpd, projectileScale);
                                    }
                                }));
                            }
                            break;
                    }

                    weapon.timerShoot = weapon.GetTotalReloadTimer() * _playerInventory.GetTotalPassiveFireRateMultiplier();
                }
            }
        }
    }

    private void ChangeGameState(GameState gameState)
    {
        _gameState = gameState;
    }
    
    private IEnumerator ShootAfterTime(float time, UnityAction callback)
    {
        yield return new WaitForSeconds(time);
        callback.Invoke();
    }
    
    public float PointRightAtTarget(Vector3 target)
    {
        Vector3 directionToTarget = target - transform.position;
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        return angle;
    }

    private Vector3 GetNearestEnemyPos()
    {
        // Obtenir tous les ennemis dans le rayon spécifié
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 20f, LayerMask.GetMask("Enemy"));

        Collider2D closest = null;
        float minDistanceSqr = Mathf.Infinity;

        foreach (Collider2D enemy in enemies)
        {
            float distanceSqr = ((Vector2)(enemy.transform.position - transform.position)).sqrMagnitude;

            if (distanceSqr < minDistanceSqr)
            {
                closest = enemy;
                minDistanceSqr = distanceSqr;
            }
        }

        return closest ? closest.transform.position : Vector3.zero;
    }
}