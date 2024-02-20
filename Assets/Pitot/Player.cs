using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float attack = 5;
    public float attackSpeed = 1;
    public int exp;
    public int level;
    public int life;
    private float lastAttackTime;

    void Update()
    {
        // Vérifier si le joueur peut attaquer
        if (Time.time - lastAttackTime >= 1 / attackSpeed)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    public void Attack()
    {
        GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        ProjectileBehavior projectileBehavior = projectileInstance.GetComponent<ProjectileBehavior>();
        if (projectileBehavior != null)
        {
            projectileBehavior.SetDamage(attack);
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
