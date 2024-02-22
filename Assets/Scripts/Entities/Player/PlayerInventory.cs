using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PlayerWeapon
{
    public WeaponPlayerSO weaponPlayerSo;
    public float timerShoot;
    public int upgradeIndex;

    public PlayerWeapon(WeaponPlayerSO weaponPlayerSo)
    {
        this.weaponPlayerSo = weaponPlayerSo;
        upgradeIndex = 0;
        timerShoot = GetTotalReloadTimer();
    }

    public void SetUpgrade(int index)
    {
        upgradeIndex = index;
    }

    public int GetTotalProjectiles()
    {
        int projectilesFromUpgrades = 0;
        if (upgradeIndex != 0)
        {
            for (int i = 0; i < upgradeIndex; i++)
            {
                projectilesFromUpgrades += weaponPlayerSo.weaponLevelArray[i].projectileToAdd;
            }
        }
        
        return weaponPlayerSo.baseProjectileNb + projectilesFromUpgrades;
    }
    
    public float GetTotalProjectileDamage()
    {
        float projectileDmgFromUpgrades = 0;
        if (upgradeIndex != 0)
        {
            for (int i = 0; i < upgradeIndex; i++)
            {
                projectileDmgFromUpgrades += weaponPlayerSo.weaponLevelArray[i].projectileDmgMultiplier;
            }
        }
        
        return weaponPlayerSo.baseProjectileDmg * (1 + projectileDmgFromUpgrades/100);
    }
    
    public float GetTotalProjectileSpd()
    {
        float projectileSpdFromUpgrades = 0;
        if (upgradeIndex != 0)
        {
            for (int i = 0; i < upgradeIndex; i++)
            {
                projectileSpdFromUpgrades += weaponPlayerSo.weaponLevelArray[i].projectileSpdMultiplier;
            }
        }
        
        return weaponPlayerSo.baseProjectileSpd * (1 + projectileSpdFromUpgrades/100);
    }
    
    public float GetTotalProjectileScale()
    {
        float projectileScaleFromUpgrades = 0;
        if (upgradeIndex != 0)
        {
            for (int i = 0; i < upgradeIndex; i++)
            {
                projectileScaleFromUpgrades += weaponPlayerSo.weaponLevelArray[i].projectileScaleMultiplier;
            }
        }
        
        return 1 * (1 + projectileScaleFromUpgrades/100);
    }

    public float GetTotalReloadTimer()
    {
        float projectileReloadFromUpgrades = 0;
        if (upgradeIndex != 0)
        {
            for (int i = 0; i < upgradeIndex; i++)
            {
                projectileReloadFromUpgrades += weaponPlayerSo.weaponLevelArray[i].weaponReloadMultiplier;
            }
        }
        return weaponPlayerSo.baseWeaponReloadTime * (1 - projectileReloadFromUpgrades/100);
    }

    public float GetTotalTimerBetweenShoot()
    {
        float projectileTimerBetweenShoot = 0;
        if (upgradeIndex != 0)
        {
            for (int i = 0; i < upgradeIndex; i++)
            {
                projectileTimerBetweenShoot += weaponPlayerSo.weaponLevelArray[i].weaponTimeBetweenShootMultiplier;
            }
        }
        
        return weaponPlayerSo.baseWeaponReloadTime * (1 - projectileTimerBetweenShoot/100);
    }

    public PlayerProjectile GetProjectilePrefab()
    {
        return upgradeIndex != 0 ? weaponPlayerSo.weaponLevelArray[upgradeIndex].projectilePrefab : weaponPlayerSo.basePlayerProjectilePrefab;
    }
}

[Serializable]
public class PlayerPassive
{
    public PassivePlayerSO passivePlayerSo;
    public int upgradeIndex;

    public PlayerPassive(PassivePlayerSO passivePlayerSo)
    {
        this.passivePlayerSo = passivePlayerSo;
        upgradeIndex = 0;
    }

    public void SetUpgrade(int index)
    {
        upgradeIndex = index;
    }
}

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] 
    private PlayerWeapon[] _playerWeaponArray;
    [SerializeField] 
    private PlayerPassive[] _playerPassiveArray;
    // [SerializeField] 
    // private PassivePlayerSO[] _playerPassiveArray;
    private int _indexWeaponInventory;
    private int _indexPassiveInventory;

    private void Start()
    {

    }

    public void AddWeaponToArray(WeaponPlayerSO weaponSO)
    {
        if (_indexWeaponInventory < _playerWeaponArray.Length)
        {
            _playerWeaponArray[_indexWeaponInventory] = new PlayerWeapon(weaponSO);
        }
        else
        {
            Debug.LogError("Too much weapons yield ");
        }

        _indexWeaponInventory++;
    }

    public void AddPassiveToArray(PassivePlayerSO passiveSO)
    {
        if (_indexPassiveInventory < _playerPassiveArray.Length)
        {
            _playerPassiveArray[_indexPassiveInventory] = new PlayerPassive(passiveSO);
            
            // Add max life 
            int maxLife = GetTotalMaxLife();
            if (maxLife != 0)
            {
                Player.Instance.AddMaxLife(maxLife);
            }
        }
        else
        {
            Debug.LogError("Too much passives yield ");
        }

        _indexPassiveInventory++;
    }

    public void ResetInventory()
    {
        
    }

    #region PassiveBuff

    public int GetTotalMaxLife()
    {
        int totalMaxLife = 0;
        int maxlifeFromUpgrades = 0;
        foreach (var passive in _playerPassiveArray)
        {
            totalMaxLife += passive.passivePlayerSo.baseMaxLife;
            if (passive.upgradeIndex != 0)
            {
                for (int i = 0; i < passive.upgradeIndex; i++)
                {
                    maxlifeFromUpgrades += passive.passivePlayerSo.passiveLevelArray[i].maxLife;
                }
            }
        }
      
        
        return totalMaxLife + maxlifeFromUpgrades;
    }
    
    public float GetTotalPassiveMovementSpeedMultiplier()
    {
        float totalMovementSpeed = 0;
        float movementSpeedFromUpgrades = 0;
        foreach (var passive in _playerPassiveArray)
        {
            totalMovementSpeed += passive.passivePlayerSo.baseMovementSpeedMultiplier;
            if (passive.upgradeIndex != 0)
            {
                for (int i = 0; i < passive.upgradeIndex; i++)
                {
                    movementSpeedFromUpgrades += passive.passivePlayerSo.passiveLevelArray[i].movementSpeedMultiplier;
                }
            }
        }
        
        return 1 + (totalMovementSpeed + movementSpeedFromUpgrades)/100;
    }
    
    public float GetTotalPassiveProjectileDamageMultiplier()
    {
        float totalDamage = 0;
        float damageFromUpgrades = 0;
        foreach (var passive in _playerPassiveArray)
        {
            totalDamage += passive.passivePlayerSo.baseProjectileDamageMultiplier;
            if (passive.upgradeIndex != 0)
            {
                for (int i = 0; i < passive.upgradeIndex; i++)
                {
                    damageFromUpgrades += passive.passivePlayerSo.passiveLevelArray[i].projectileDamageMultiplier;
                }
            }
        }


        return 1 + (totalDamage + damageFromUpgrades) / 100;
    }

    public float GetTotalPassiveFireRateMultiplier()
    {
        float fireRate = 0;
        float fireRateFromUpgrades = 0;
        foreach (var passive in _playerPassiveArray)
        {
            fireRate += passive.passivePlayerSo.baseWeaponFireRateMultiplier;
            if (passive.upgradeIndex != 0)
            {
                for (int i = 0; i < passive.upgradeIndex; i++)
                {
                    fireRateFromUpgrades += passive.passivePlayerSo.passiveLevelArray[i].weaponFireRateMultiplier;
                }
            }
        }

        return 1 - (fireRate + fireRateFromUpgrades)/100;
    }

    public float GetTotalPassiveProjectileSpeed()
    {
        float projectileSpeed = 0;
        float projectileSpeedFromUpgrades = 0;
        foreach (var passive in _playerPassiveArray)
        {
            projectileSpeed += passive.passivePlayerSo.baseProjectileSpdMultiplier;
            if (passive.upgradeIndex != 0)
            {
                for (int i = 0; i < passive.upgradeIndex; i++)
                {
                    projectileSpeedFromUpgrades += passive.passivePlayerSo.passiveLevelArray[i].projectileSpdMultiplier;
                }
            }
        }

        
        return 1 + (projectileSpeed + projectileSpeedFromUpgrades)/100;
    }
    
    public float GetTotalPassiveProjectileScale()
    {
        float projectileScale = 0;
        float projectileScaleFromUpgrades = 0;
        foreach (var passive in _playerPassiveArray)
        {
            projectileScale += passive.passivePlayerSo.baseProjectileScaleMultiplier;
            if (passive.upgradeIndex != 0)
            {
                for (int i = 0; i < passive.upgradeIndex; i++)
                {
                    projectileScaleFromUpgrades += passive.passivePlayerSo.passiveLevelArray[i].projectileScaleMultiplier;
                }
            }
        }
        
        return 1 + (projectileScale + projectileScaleFromUpgrades)/100;
    }

    #endregion
  

    public PlayerWeapon[] PlayerWeaponArray => _playerWeaponArray;

    public PlayerPassive[] PlayerPassiveArray => _playerPassiveArray;
}