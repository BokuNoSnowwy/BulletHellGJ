using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : BaseProjectile
{
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Player>().TakeDamage(_projectileDmg);
        }
    }
    
    private void OnBecameInvisible()
    {
        _poolingManager.EnemyProjectilesPool.Release(this);
    }
}
