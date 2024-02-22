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
         _poolingManager.PlayerProjectilesPool.Release(this);
         col.gameObject.GetComponent<Enemy>().TakeDamage(_projectileDmg);
      }
   }

   protected override void ReleaseFromPool()
   {
      base.ReleaseFromPool();
      _poolingManager.PlayerProjectilesPool.Release(this);
   }
}
