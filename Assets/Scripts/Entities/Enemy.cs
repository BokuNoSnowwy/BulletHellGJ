using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public float attackDamage = 5;

    [SerializeField]
    protected SpriteRenderer _sprite;
    //TODO Put dropable prefab
    [SerializeField]
    private GameObject _xpPrefab;   
    [SerializeField]
    private float timerMaxImmunityPlayer;
    private float timerImmunityPlayer;
    private bool canDamagePlayer;

    protected Player _player;
    private GameManager _gameManager;

    //Keep the enemy from damaging player every tick
    [SerializeField]

    // Start is called before the first frame update
    void Start()
    {
        Initialization();
    }

    protected void Initialization()
    {
        _gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        Tick();
    }

    protected void Tick()
    {
        if (_gameManager.GameState != GameState.Game)
        {
            return;
        }

        if (!canDamagePlayer)
        {
            timerImmunityPlayer -= Time.deltaTime;
            if (timerImmunityPlayer <= 0)
            {
                canDamagePlayer = true;
            }
        }
    }
    
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        //TODO Disable enemy controller
        //TODO Death animation 
        //TODO Drop XP
        ObjectsPoolingManager.Instance.EnemiesPool.Release(gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        CheckCollisionPlayer(col);
    }

    protected void OnTriggerStay2D(Collider2D col)
    {
        CheckCollisionPlayer(col);
    }
    
    private void TouchPlayer()
    {
        _player.TakeDamage(attackDamage);
        timerImmunityPlayer = timerMaxImmunityPlayer;
        canDamagePlayer = false;
    }

    private void CheckCollisionPlayer(Collider2D col)
    {
        Debug.LogError("CheckCollisionPlayer");
        if (!col.gameObject.CompareTag("Player"))
        {
            return;
        }
        
        if (_player = null)
        {
            _player = col.gameObject.GetComponent<Player>();
        }
        
        if (canDamagePlayer)
        {
            TouchPlayer();
        }
    }
}
