using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponProjectileType
{
    TargetAtNearestEnemy,
    FollowPlayerDirection,
    EveryDirection
}
[CreateAssetMenu(fileName = "WeaponPlayer", menuName = "ScriptableObjects/Weapons/WeaponPlayerSO", order = 1)]
public class WeaponPlayerSO : ItemPlayerSO
{
    [Header("Weapon Stats")]
    public WeaponProjectileType projectileType;
    public int baseProjectileNb;
    public float baseProjectileDmg;
    public float baseProjectileSpd;
    public float baseWeaponReloadTime;
    public float baseTimeBetweenShoot;
    public PlayerProjectile basePlayerProjectilePrefab;
    public WeaponUpgradeSO[] weaponLevelArray;
}
