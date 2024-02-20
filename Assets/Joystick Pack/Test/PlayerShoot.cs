using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject projectilePrefab; 
    public Transform shootPoint; 
    public float projectileSpeed = 10f;
    public float shootInterval = 1f; 

    private float lastShootTime;

    public bool useShootA;
    public bool useShoot360;

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


    public int numProjectiles = 10; // Le nombre de projectiles à tirer

    // Méthode pour tirer pour le shoot "Arme qui tire dans une direction fixe"
    public void Shoot360()
    {
        
        float angleStep = 360f / numProjectiles;

        // Boucle pour instancier chaque projectile
        for (int i = 0; i < numProjectiles; i++)
        {
            
            Quaternion rotation = Quaternion.Euler(-90f, angleStep * i, 0f);

            
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, rotation);

            
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            
            if (rb != null)
            {
                rb.velocity = projectile.transform.forward * projectileSpeed;
            }
            else
            {
                Debug.LogWarning("Projectile prefab does not have a Rigidbody component!");
            }
        }
    }

   
    
}
