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
      }
   }
   
   private void OnBecameInvisible()
   {
      _poolingManager.PlayerProjectilesPool.Release(this);
   }
}
