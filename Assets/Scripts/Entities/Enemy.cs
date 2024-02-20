using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : Entity
{
    [Header("Enemy Stats")]
    public float attackDamage = 5;

    [SerializeField]
    protected SpriteRenderer _sprite;
    //TODO Put dropable prefab
    [SerializeField]
    private GameObject _xpPrefab;   
    [SerializeField]
    private float _timerMaxImmunityPlayer;
    private float _timerImmunityPlayer;
    private bool _canDamagePlayer;

    protected Player _player;
    private GameManager _gameManager;
    
    protected void Initialization()
    {
        _gameManager = GameManager.Instance;
    }

    protected void Tick()
    {
        if (_gameManager.GameState != GameState.Game)
        {
            return;
        }

        if (!_canDamagePlayer)
        {
            _timerImmunityPlayer -= Time.deltaTime;
            if (_timerImmunityPlayer <= 0)
            {
                _canDamagePlayer = true;
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        CheckCollisionPlayer(col);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        CheckCollisionPlayer(col);
    }
    
    private void TouchPlayer()
    {
        _player.TakeDamage(attackDamage);
        _timerImmunityPlayer = _timerMaxImmunityPlayer;
        _canDamagePlayer = false;
    }

    private void CheckCollisionPlayer(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player"))
        {
            return;
        }
        
        if (_player = null)
        {
            _player = col.gameObject.GetComponent<Player>();
        }
        
        if (_canDamagePlayer)
        {
            TouchPlayer();
        }
    }
}
