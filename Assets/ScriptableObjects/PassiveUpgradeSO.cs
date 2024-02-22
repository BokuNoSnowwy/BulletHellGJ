using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveUpgrade", menuName = "ScriptableObjects/Passives/PassiveUpgradeSO", order = 1)]
public class PassiveUpgradeSO : ScriptableObject
{
    [Header("Passive Player Info")]
    public string upgradeDescription;
    
    [Header("Passive Stats")]
    public int maxLife;
    public float movementSpeedMultiplier;
    public float projectileDamageMultiplier;
    public float weaponFireRateMultiplier;
    public float projectileSpdMultiplier;
    public float projectileScaleMultiplier;
}
