using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingShootingEnemy : Enemy
{
    [Header("Bullet")]
    [SerializeField] 
    private GameObject _bulletPrefab;
    [SerializeField] 
    private float _distanceShoot;
    [SerializeField] 
    private float _timerMaxShoot;
    private float _timerShoot;

    private bool _canShootTowardPlayer;
    // Start is called before the first frame update
    void Start()
    {
        Initialization();
        _player = Player.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        Tick();
        
        // Flip update
        _sprite.flipX = _player.transform.position.x < transform.position.x;

        Vector3 playerPos = _player.transform.position;
        Vector3 playerDir = playerPos - transform.position;
        transform.Translate(playerDir.normalized * movementSpeed * Time.deltaTime);

        // Shooting reload
        if (!_canShootTowardPlayer)
        {
            _timerShoot -= Time.deltaTime;
            if (_timerShoot <= 0)
            {
                _canShootTowardPlayer = true;
                _timerShoot = _timerMaxShoot;
            }
        }
        
        if (Vector3.Distance(transform.position, playerPos) <= _distanceShoot)
        {
            //Shoot
            if (_canShootTowardPlayer)
            {
                Shoot(playerPos);
            }
        }
    }

    void Shoot(Vector3 target)
    {
        _canShootTowardPlayer = false;
        Debug.Log("Instanciate Bullet ");
        
        //TODO Pooling of the bullet
        //TODO Setup direction toward player
        
    }

    
}
