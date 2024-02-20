using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponProjectileType
{
    TargetAtNearestEnemy,
    FollowPlayerDirection,
    EveryDirection
}
[CreateAssetMenu(fileName = "WeaponPlayer", menuName = "ScriptableObjects/WeaponPlayerSO", order = 1)]
public class WeaponPlayerSO : ScriptableObject
{
    [Header("Weapon Player Info")]
    public string weaponName;
    public Sprite weaponSprite;
    public string weaponDescription;

    [Header("Weapon Stats")]
    public WeaponProjectileType projectileType;
    public int baseProjectileNb;
    public float baseProjectileDmg;
    public float baseProjectileSpd;
    public float baseWeaponReloadTime;
    public WeaponUpgradeSO[] weaponLevelArray;
}
