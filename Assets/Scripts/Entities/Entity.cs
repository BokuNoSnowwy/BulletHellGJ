using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Entity : MonoBehaviour
{
    [FormerlySerializedAs("maxLife")]
    [Header("Entity Stats")]
    [SerializeField]
    protected float _maxLife;
    [FormerlySerializedAs("movementSpeed")] [SerializeField]
    protected float _movementSpeed;
    
    protected float _life;



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
        _life = _maxLife;
    }

    public virtual bool IsAlive()
    {
        return _life > 0f;
    }

    public virtual void TakeDamage(float damage)
    {
        _life -= Mathf.RoundToInt(damage);
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
