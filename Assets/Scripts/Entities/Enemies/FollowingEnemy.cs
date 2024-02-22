using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingEnemy : Enemy
{
    void Start()
    {
        //Initialization();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_gameState == GameState.Pause)
        {
            Debug.LogError("LogEnemy");
            return;
        }
        
        Tick();
        // Flip update
        _spriteRenderer.flipX = _player.transform.position.x < transform.position.x;

        Vector3 playerDir = _player.transform.position - transform.position;
        transform.Translate(playerDir.normalized * _movementSpeed * Time.deltaTime);
    }
}
