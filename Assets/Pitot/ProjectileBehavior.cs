using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private float damage = 0; // Dégâts infligés par le projectile
    private float speed = 10f; // Vitesse du projectile

    // Méthode pour définir les dégâts du projectile
    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    void Update()
    {
        // Déplacer le projectile vers l'avant
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Vérifier si le projectile entre en collision avec un ennemi
        /*Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            // Infliger des dégâts à l'ennemi
            enemy.TakeDamage(damage);
        }*/

        // Détruire le projectile après la collision
        Destroy(gameObject);
    }
}
