using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : BaseProjectile
{
   protected override void OnTriggerEnter2D(Collider2D col)
   {
      if (col.CompareTag("Enemy"))
      {
         col.gameObject.GetComponent<Enemy>().TakeDamage(_projectileDmg);
         _poolingManager.PlayerProjectilesPool.Release(this);
      }
   }

   protected override void ReleaseFromPool()
   {
      base.ReleaseFromPool();
      _poolingManager.PlayerProjectilesPool.Release(this);
   }
}
