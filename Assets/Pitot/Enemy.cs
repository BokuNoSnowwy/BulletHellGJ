using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 10; // Sant� maximale de l'ennemi
    private int currentHealth; // Sant� actuelle de l'ennemi

    void Start()
    {
        currentHealth = maxHealth; // Initialiser la sant� actuelle � la sant� maximale au d�marrage
    }

    // M�thode pour prendre des d�g�ts
    public void TakeDamage(float damage)
    {
        currentHealth -= Mathf.RoundToInt(damage);
        if (currentHealth <= 0)
        {
            Die(); // Appeler la m�thode Die si l'ennemi est mort
        }
    }

    // M�thode pour v�rifier si l'ennemi est encore en vie
    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    // M�thode pour g�rer la mort de l'ennemi
    private void Die()
    {
        // Impl�mentez votre logique de mort ici, par exemple, jouer une animation de mort, d�truire l'ennemi, etc.
        Destroy(gameObject);
    }

    // Autres m�thodes et fonctions n�cessaires peuvent �tre ajout�es ici en fonction des besoins de votre jeu
}
