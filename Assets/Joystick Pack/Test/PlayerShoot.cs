using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[Serializable]
public class PlayerWeapon
{
    public WeaponPlayerSO weaponPlayerSo;
    public float timerShoot;
    public int upgradeIndex;

    public PlayerWeapon(WeaponPlayerSO weaponPlayerSo)
    {
        this.weaponPlayerSo = weaponPlayerSo;
        upgradeIndex = 0;
        timerShoot = GetTotalReloadTimer();
    }

    public void SetUpgrade(int index)
    {
        upgradeIndex = index;
    }

    public int GetTotalProjectiles()
    {
        int projectilesFromUpgrades = 0;
        if (upgradeIndex != 0)
        {
            for (int i = 0; i < upgradeIndex-1; i++)
            {
                projectilesFromUpgrades += weaponPlayerSo.weaponLevelArray[i].projectileToAdd;
            }
        }
        
        return weaponPlayerSo.baseProjectileNb + projectilesFromUpgrades;
    }
    
    public float GetTotalProjectileDamage()
    {
        float projectileDmgFromUpgrades = 0;
        if (upgradeIndex != 0)
        {
            for (int i = 0; i < upgradeIndex-1; i++)
            {
                projectileDmgFromUpgrades += weaponPlayerSo.weaponLevelArray[i].projectileDmgMultiplier;
            }
        }
        
        return weaponPlayerSo.baseProjectileDmg * (1 + projectileDmgFromUpgrades/100);
    }
    
    public float GetTotalProjectileSpd()
    {
        float projectileSpdFromUpgrades = 0;
        if (upgradeIndex != 0)
        {
            for (int i = 0; i < upgradeIndex-1; i++)
            {
                projectileSpdFromUpgrades += weaponPlayerSo.weaponLevelArray[i].projectileSpdMultiplier;
            }
        }
        
        return weaponPlayerSo.baseProjectileSpd * (1 + projectileSpdFromUpgrades/100);
    }
    
    public float GetTotalProjectileScale()
    {
        float projectileScaleFromUpgrades = 0;
        if (upgradeIndex != 0)
        {
            for (int i = 0; i < upgradeIndex-1; i++)
            {
                projectileScaleFromUpgrades += weaponPlayerSo.weaponLevelArray[i].projectileScaleMultiplier;
            }
        }
        
        return 1 * (1 + projectileScaleFromUpgrades/100);
    }

    public float GetTotalReloadTimer()
    {
        float projectileReloadFromUpgrades = 0;
        if (upgradeIndex != 0)
        {
            for (int i = 0; i < upgradeIndex-1; i++)
            {
                projectileReloadFromUpgrades += weaponPlayerSo.weaponLevelArray[i].weaponReloadMultiplier;
            }
        }
        return weaponPlayerSo.baseWeaponReloadTime * (1 - projectileReloadFromUpgrades/100);
    }

    public float GetTotalTimerBetweenShoot()
    {
        float projectileTimerBetweenShoot = 0;
        if (upgradeIndex != 0)
        {
            for (int i = 0; i < upgradeIndex-1; i++)
            {
                projectileTimerBetweenShoot += weaponPlayerSo.weaponLevelArray[i].weaponTimeBetweenShootMultiplier;
            }
        }
        
        return weaponPlayerSo.baseWeaponReloadTime * (1 - projectileTimerBetweenShoot/100);
    }

    public PlayerProjectile GetProjectilePrefab()
    {
        return upgradeIndex != 0 ? weaponPlayerSo.weaponLevelArray[upgradeIndex].projectilePrefab : weaponPlayerSo.basePlayerProjectilePrefab;
    }
}
public class PlayerShoot : MonoBehaviour
{
    [HideInInspector]
    public GameObject projectilePrefab; 
    [HideInInspector]
    public Transform shootPoint; 
    [HideInInspector]
    public float projectileSpeed = 10f;
    [HideInInspector]
    public float shootInterval = 4f; 

    private float lastShootTime;
    
    private bool useShootA;
    private bool useShoot360;
    private bool useShootTarget;

    [HideInInspector]
    public float detectionRadius = 10f;
    
    //new Weapon system
    [SerializeField]
    private Transform shootingTransform;
    [SerializeField]
    private PlayerWeapon[] _playerWeapons = new PlayerWeapon[3];

    private ObjectsPoolingManager _poolingManager;
    

    void Start()
    {
        lastShootTime = -shootInterval; 
        _poolingManager = ObjectsPoolingManager.Instance;
    }

    void Update()
    {
        //nearest enemy 
        
        foreach (var weapon in _playerWeapons)
        {
            weapon.timerShoot -= Time.deltaTime;
            if (weapon.timerShoot <= 0)
            {
                int projectileNb = weapon.GetTotalProjectiles();
                float projectileDmg = weapon.GetTotalProjectileDamage();
                float projectileSpd = weapon.GetTotalProjectileSpd();
                float projectileScale = weapon.GetTotalProjectileScale();
                
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

                weapon.timerShoot = weapon.GetTotalReloadTimer();
            }
        }
    }

    private IEnumerator ShootAfterTime(float time, UnityAction callback)
    {
        Debug.LogError(time);
        yield return new WaitForSeconds(time);
        Debug.LogError("After waiting ");
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

    // M�thode pour tirer pour le shoot "Arme qui tire en direction du Joystick"
    void Shoot()
    {
        
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.Euler(-90f, 0f, 0f));


        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        
        if (rb != null)
        {
            rb.velocity = shootPoint.forward * projectileSpeed;
        }
        else
        {
            Debug.LogWarning("Projectile prefab does not have a Rigidbody component!");
        }
    }


    public int numProjectiles = 10; 

    // M�thode pour tirer pour le shoot "Arme qui tire dans une direction fixe"
    public void Shoot360()
    {

        float angleStep = 360f / numProjectiles;

        // Boucle pour instancier chaque projectile
        for (int i = 0; i < numProjectiles; i++)
        {

            Quaternion rotation = Quaternion.Euler(-90f, angleStep * i, 0f);


            GameObject projectile = Instantiate(projectilePrefab, this.transform.position, rotation);


            Rigidbody rb = projectile.GetComponent<Rigidbody>();


            if (rb != null)
            {
                rb.velocity = projectile.transform.right * projectileSpeed;
            }
            else
            {
                Debug.LogWarning("Projectile prefab does not have a Rigidbody component!");
            }
        }
    }

    // M�thode pour tirer pour le shoot "Arme qui tire sur les ennemis proches"
    void ShootAt(Vector3 targetPosition)
    {
        // Cr�er une instance du projectile � partir de la pr�fabriqu�e � la position du joueur
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(-90f, 0f, 0f));

        // Calculer la direction du tir
        Vector3 shootDirection = (targetPosition - transform.position).normalized;

        // R�cup�rer le Rigidbody du projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Appliquer la v�locit� au projectile dans la direction du tir
        if (rb != null)
        {
            rb.velocity = shootDirection * projectileSpeed;
        }
        else
        {
            Debug.LogWarning("Projectile prefab does not have a Rigidbody component!");
        }
    }

}
