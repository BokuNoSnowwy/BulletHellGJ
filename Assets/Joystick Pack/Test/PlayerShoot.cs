using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class PlayerWeapon
{
    public WeaponPlayerSO weaponPlayerSo;
    public float timerMaxShoot;
    public float timerShoot;

    public PlayerWeapon(WeaponPlayerSO weaponPlayerSo, float timerMaxShoot)
    {
        this.weaponPlayerSo = weaponPlayerSo;
        this.timerMaxShoot = timerMaxShoot;
        timerShoot = timerMaxShoot;
    }
}
public class PlayerShoot : MonoBehaviour
{
    public GameObject projectilePrefab; 
    public Transform shootPoint; 
    public float projectileSpeed = 10f;
    public float shootInterval = 4f; 

    private float lastShootTime;

    [SerializeField]
    private bool useShootA;
    [SerializeField]
    private bool useShoot360;
    [SerializeField]
    private bool useShootTarget;

    public float detectionRadius = 10f;
    
    //new Weapon system
    private PlayerWeapon[] _playerWeapons = new PlayerWeapon[3];
    
    

    void Start()
    {
        lastShootTime = -shootInterval; 
    }

    void Update()
    {
        //nearest enemy 
        
        foreach (var weapon in _playerWeapons)
        {
            weapon.timerShoot -= Time.deltaTime;
            if (weapon.timerShoot <= 0)
            {
                switch (weapon.weaponPlayerSo.projectileType)
                {
                    case WeaponProjectileType.EveryDirection :
                        //TODO Get the number of projectile and divide it from 360 degres 
                        break;
                    case WeaponProjectileType.FollowPlayerDirection : 
                        //TODO Multiple projectiles in a row (intern timers)
                        break;
                    case WeaponProjectileType.TargetAtNearestEnemy : 
                        //TODO Multiple projectiles in a row (intern timers)    
                        break;
                }
                weapon.timerShoot = weapon.timerMaxShoot;
            }
        }
        
        // if (Time.time - lastShootTime >= shootInterval) 
        // {
        //     if (useShootA)
        //     {
        //         Shoot();
        //     }
        //     if (useShoot360)
        //     {
        //         Shoot360();
        //     }
        //     if (useShootTarget)
        //     {
        //         // D�tection des ennemis les plus proches
        //         Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        //
        //         GameObject nearestEnemy = null;
        //         float nearestDistance = Mathf.Infinity;
        //
        //         foreach (Collider col in colliders)
        //         {
        //             // V�rifiez si le collider appartient � un ennemi
        //             if (col.CompareTag("Enemy"))
        //             {
        //
        //                 float distanceToEnemy = Vector3.Distance(transform.position, col.transform.position);
        //                 if (distanceToEnemy < nearestDistance)
        //                 {
        //                     nearestEnemy = col.gameObject;
        //                     nearestDistance = distanceToEnemy;
        //                 }
        //             }
        //         }
        //
        //         // Tirer sur l'ennemi le plus proche
        //         if (nearestEnemy != null)
        //         {
        //             ShootAt(nearestEnemy.transform.position);
        //         }
        //     }
        //
        //     lastShootTime = Time.time; 
        // }

        
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
