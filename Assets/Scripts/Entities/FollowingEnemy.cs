using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingEnemy : Enemy
{
    void Start()
    {
        Initialization();
    }
    
    // Update is called once per frame
    void Update()
    {
        Tick();
        
        // Flip update
        _sprite.flipX = _player.transform.position.x < transform.position.x;

        Vector3 playerDir = _player.transform.position - transform.position;
        transform.Translate(playerDir.normalized * movementSpeed * Time.deltaTime);
    }
}
