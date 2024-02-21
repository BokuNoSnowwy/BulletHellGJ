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
            _poolingManager.EnemyProjectilesPool.Release(this);
        }
    }
    
    protected override void ReleaseFromPool()
    {
        base.ReleaseFromPool();
        _poolingManager.EnemyProjectilesPool.Release(this);
    }
}
