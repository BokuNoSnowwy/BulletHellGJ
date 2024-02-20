using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 10; // Santé maximale de l'ennemi
    private int currentHealth; // Santé actuelle de l'ennemi

    void Start()
    {
        currentHealth = maxHealth; // Initialiser la santé actuelle à la santé maximale au démarrage
    }

    // Méthode pour prendre des dégâts
    public void TakeDamage(float damage)
    {
        currentHealth -= Mathf.RoundToInt(damage);
        if (currentHealth <= 0)
        {
            Die(); // Appeler la méthode Die si l'ennemi est mort
        }
    }

    // Méthode pour vérifier si l'ennemi est encore en vie
    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    // Méthode pour gérer la mort de l'ennemi
    private void Die()
    {
        // Implémentez votre logique de mort ici, par exemple, jouer une animation de mort, détruire l'ennemi, etc.
        Destroy(gameObject);
    }

    // Autres méthodes et fonctions nécessaires peuvent être ajoutées ici en fonction des besoins de votre jeu
}
