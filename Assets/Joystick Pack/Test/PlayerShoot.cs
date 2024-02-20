using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        lastShootTime = -shootInterval; 
    }

    void Update()
    {
        if (Time.time - lastShootTime >= shootInterval) 
        {
            if (useShootA)
            {
                Shoot();
            }
            if (useShoot360)
            {
                Shoot360();
            }
            if (useShootTarget)
            {
                // Détection des ennemis les plus proches
                Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

                GameObject nearestEnemy = null;
                float nearestDistance = Mathf.Infinity;

                foreach (Collider col in colliders)
                {
                    // Vérifiez si le collider appartient à un ennemi
                    if (col.CompareTag("Enemy"))
                    {

                        float distanceToEnemy = Vector3.Distance(transform.position, col.transform.position);
                        if (distanceToEnemy < nearestDistance)
                        {
                            nearestEnemy = col.gameObject;
                            nearestDistance = distanceToEnemy;
                        }
                    }
                }

                // Tirer sur l'ennemi le plus proche
                if (nearestEnemy != null)
                {
                    ShootAt(nearestEnemy.transform.position);
                }
            }

            lastShootTime = Time.time; 
        }

        
    }

    // Méthode pour tirer pour le shoot "Arme qui tire en direction du Joystick"
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

    // Méthode pour tirer pour le shoot "Arme qui tire dans une direction fixe"
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

    // Méthode pour tirer pour le shoot "Arme qui tire sur les ennemis proches"
    void ShootAt(Vector3 targetPosition)
    {
        // Créer une instance du projectile à partir de la préfabriquée à la position du joueur
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(-90f, 0f, 0f));

        // Calculer la direction du tir
        Vector3 shootDirection = (targetPosition - transform.position).normalized;

        // Récupérer le Rigidbody du projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Appliquer la vélocité au projectile dans la direction du tir
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
