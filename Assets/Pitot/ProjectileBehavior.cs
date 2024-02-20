using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private float damage = 0; // D�g�ts inflig�s par le projectile
    private float speed = 10f; // Vitesse du projectile

    // M�thode pour d�finir les d�g�ts du projectile
    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    void Update()
    {
        // D�placer le projectile vers l'avant
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // V�rifier si le projectile entre en collision avec un ennemi
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            // Infliger des d�g�ts � l'ennemi
            enemy.TakeDamage(damage);
        }

        // D�truire le projectile apr�s la collision
        Destroy(gameObject);
    }
}
