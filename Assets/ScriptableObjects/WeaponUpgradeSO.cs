using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponUpgrade", menuName = "ScriptableObjects/WeaponUpgradeSO", order = 1)]
public class WeaponUpgradeSO : ScriptableObject
{
    [Header("Weapon Player Info")]
    public string upgradeName;
    public Sprite upgradeSprite;
    public string upgradeDescription;
    
    [Header("Weapon Stats")]
    public int projectileToAdd;
    public float projectileDmgMultiplier;
    public float projectileScaleMultiplier;
    public float projectileSpdMultiplier;
    public float weaponReloadMultiplier;
    public GameObject projectilePrefab;
}
