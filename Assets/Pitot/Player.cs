using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject projectilePrefab;

    public float attack;
    public float attackSpeed;
    public int exp;
    public int level;
    public int life;
    public float lastAttackTime;

    private bool isAttacking = false; // Indique si le joueur est en train d'attaquer
    private WaitForSeconds attackCooldown; // Temps d'attente entre chaque attaque

    void Start()
    {
        // Initialiser le temps d'attente entre chaque attaque
        attackCooldown = new WaitForSeconds(1f / attackSpeed);

        // Démarrer l'attaque automatique
        StartCoroutine(AttackRoutine());
    }

    // Coroutine pour gérer l'attaque automatique
    IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (!isAttacking)
            {
                Attack();
            }
            yield return attackCooldown;
        }
    }

    public void Attack()
    {
        if (Time.time - lastAttackTime >= 1 / attackSpeed)
        {
            lastAttackTime = Time.time;

            // Instancier le projectile devant le joueur
            GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            // Vous pouvez ajuster la position de l'instance du projectile en fonction de l'orientation du joueur

            // Récupérer le composant du projectile s'il a un script pour lui passer des informations
            ProjectileBehavior projectileBehavior = projectileInstance.GetComponent<ProjectileBehavior>();
            if (projectileBehavior != null)
            {
                projectileBehavior.SetDamage(attack); // Passer les dégâts du joueur au projectile
            }
        }
    }

    public void GainExp(int amount)
    {
        exp += amount;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (exp >= 10)
        {
            level++;
            exp = 0;
            Debug.Log("Level Up! Current Level: " + level);
        }
    }

    public bool IsAlive()
    {
        return life > 0;
    }

    public void TakeDamage(float damage)
    {
        life -= Mathf.RoundToInt(damage);
        if (!IsAlive())
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("You Died");
    }
}
