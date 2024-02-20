using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity Stats")]
    [SerializeField]
    protected float maxLife;
    [SerializeField]
    protected float movementSpeed;
    
    protected float life;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnEnable()
    {
        life = maxLife;
    }

    public virtual bool IsAlive()
    {
        return life > 0f;
    }

    public virtual void TakeDamage(float damage)
    {
        life -= Mathf.RoundToInt(damage);
        if (!IsAlive())
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log("You Died");
    }
}
