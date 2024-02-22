using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : Entity
{
    [Header("Enemy Stats")]
    private float _attackDamage = 5;

    [SerializeField]
    protected SpriteRenderer _spriteRenderer;
    [SerializeField]
    protected Animator _animator;

    [SerializeField]
    private float _timerMaxImmunityPlayer;

    private float _timerImmunityPlayer;

    private bool _canDamagePlayer;

    public Score score;

    protected Player _player;
    protected ObjectsPoolingManager _poolingManager;
    private GameManager _gameManager;
    protected GameState _gameState;

    [SerializeField] protected EnemyParameters _parameters;

    public void Initialization(EnemyParameters parameters)
    {
        _parameters = parameters; 

        _gameManager = GameManager.Instance;
        _poolingManager = ObjectsPoolingManager.Instance;
        _gameState = _gameManager.GameState;
        _gameManager.AddGameStateChangeListener(ChangeGameState);
        _player = Player.Instance;

        if (_parameters != null)
        {
            _movementSpeed = _parameters.Speed;
            _maxLife = _parameters.MaxLife;
            _life = _parameters.MaxLife;
            _attackDamage = _parameters.AttackDamage;

            _animator.runtimeAnimatorController = _parameters.SpritesAnimation;
          
        }

        gameObject.SetActive(true);
    }


    public void OnTakenFromPool()
    {
        /*
        Initialization(parameters);
        gameObject.SetActive(true);
        */
    }

    protected void Tick()
    {
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

    private void ChangeGameState(GameState gameState)
    {
        _gameState = gameState;
    }

    protected override void Die()
    {
        //TODO Disable enemy controller
        //TODO Death animation 
        score.score++;
        ExperiencePoint expPoint = ObjectsPoolingManager.Instance.ExpPool.Get();
        expPoint.transform.position = transform.position;
        ObjectsPoolingManager.Instance.EnemiesPool.Release(this);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        CheckCollisionPlayer(col);
        CheckOutOfBounds(col);
    }

    private void CheckOutOfBounds(Collider2D col)
    {
        if (!col.gameObject.CompareTag("DeathZone"))
        {
            return;
        } else
        {
            ObjectsPoolingManager.Instance.EnemiesPool.Release(this);
            Debug.Log("Enemy OOB");
        }

    }

    private void OnTriggerStay2D(Collider2D col)
    {
        CheckCollisionPlayer(col);
    }
    
    private void TouchPlayer()
    {
        _player.TakeDamage(_attackDamage);
        _timerImmunityPlayer = _timerMaxImmunityPlayer;
        _canDamagePlayer = false;
    }

    private void CheckCollisionPlayer(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player"))
        {
            return;
        }
        
        if (_player == null)
        {
            _player = col.gameObject.GetComponent<Player>();
        }
        
        if (_canDamagePlayer)
        {
            TouchPlayer();
            
        }
    }
}
