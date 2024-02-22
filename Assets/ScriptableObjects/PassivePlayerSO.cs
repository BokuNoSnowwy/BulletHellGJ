using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassivePlayer", menuName = "ScriptableObjects/Passives/PassivePlayerSO", order = 1)]
public class PassivePlayerSO : ItemPlayerSO
{
    [Header("Passive Stats")]
    public int baseMaxLife;
    public float baseMovementSpeedMultiplier;
    public float baseProjectileDamageMultiplier;
    public float baseWeaponFireRateMultiplier;
    public float baseProjectileSpdMultiplier;
    public float baseProjectileScaleMultiplier;
    public PassiveUpgradeSO[] passiveLevelArray;
}
