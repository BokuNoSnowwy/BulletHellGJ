using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


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
    [Header("Items Array")]
    [SerializeField] 
    private PlayerWeapon[] _playerWeaponArray;
    [SerializeField] 
    private PlayerPassive[] _playerPassiveArray;
    
    private int _indexWeaponInventory;
    private int _indexPassiveInventory;

    [Header("XP Bonus")]
    [SerializeField]
    private ItemPlayerSO[] _itemGame;
    [SerializeField]
    private ItemChoice[] _itemChoiceArray = new ItemChoice[3];

    private void Start()
    {

    }

    public void AddWeaponToArray(WeaponPlayerSO weaponSO)
    {
        PlayerWeapon playerWeapon = _playerWeaponArray.ToList().Find(weapon => weaponSO == weapon.weaponPlayerSo);
        
        if (playerWeapon != null)
        {
            // Upgrade
            playerWeapon.upgradeIndex++;
            
            //Check fully upgraded
            if (playerWeapon.upgradeIndex > playerWeapon.weaponPlayerSo.weaponLevelArray.Length)
            {
                _itemGame.ToList().Remove(playerWeapon.weaponPlayerSo);
            }
        }
        else
        {
            // Add new
            if (CheckIfWeaponHasEmpty())
            {
                _playerWeaponArray[_indexWeaponInventory] = new PlayerWeapon(weaponSO);
                // Add max life 
                int maxLife = GetTotalMaxLife();
                if (maxLife != 0)
                {
                    Player.Instance.AddMaxLife(maxLife);
                }
                
                _indexWeaponInventory++;
            }
            else
            {
                Debug.LogError(_itemGame.Length);
                _itemGame.ToList().RemoveAll(weapon => (WeaponPlayerSO) weapon == _playerWeaponArray.ToList().Find(playerWeapon => playerWeapon.weaponPlayerSo != (WeaponPlayerSO) weapon).weaponPlayerSo);
                Debug.LogError(_itemGame.Length);
            }
        }
    }

    public void AddPassiveToArray(PassivePlayerSO passiveSO)
    {
        PlayerPassive playerPassive = _playerPassiveArray.ToList().Find(passive => passiveSO == passive.passivePlayerSo);
        
        if (playerPassive != null)
        {
            // Upgrade
            playerPassive.upgradeIndex++;
            
            //Check fully upgraded
            if (playerPassive.upgradeIndex > playerPassive.passivePlayerSo.passiveLevelArray.Length)
            {
                _itemGame.ToList().Remove(playerPassive.passivePlayerSo);
            }
        }
        else
        {
            // Add new
            if (CheckIfPassiveHasEmpty())
            {
                _playerPassiveArray[_indexPassiveInventory] = new PlayerPassive(passiveSO);
                // Add max life 
                int maxLife = GetTotalMaxLife();
                if (maxLife != 0)
                {
                    Player.Instance.AddMaxLife(maxLife);
                }
                
                _indexPassiveInventory++;
            }
            else
            {
                Debug.LogError(_itemGame.Length);
                _itemGame.ToList().RemoveAll(passive => (PassivePlayerSO) passive == _playerPassiveArray.ToList().Find(playerPassive => playerPassive.passivePlayerSo != (PassivePlayerSO) passive).passivePlayerSo);
                Debug.LogError(_itemGame.Length);
            }
        }
    }

    public void DisplayUpgrades(UnityAction callbackClosePanel)
    {
        List<ItemPlayerSO> itemArray = new List<ItemPlayerSO>(_itemGame);
        
        ItemPlayerSO[] newItemArray = new ItemPlayerSO[3];

        for (int i = 0; i < newItemArray.Length; i++)
        {
            ItemPlayerSO itemPlayer = itemArray[Random.Range(0, itemArray.Count)];
            newItemArray[i] = itemPlayer;
            itemArray.Remove(itemPlayer);
        }
        
        for (int i = 0; i < _itemChoiceArray.Length; i++)
        {
            string itemName = newItemArray[i].itemName;
            string itemDescription = newItemArray[i].itemDescription;

            //Check if item already in array, then 
            if (newItemArray.GetType() == typeof(WeaponPlayerSO))
            {
                foreach (var playerWeapon in _playerWeaponArray)
                {
                    if (playerWeapon.weaponPlayerSo == newItemArray[i])
                    {
                        itemName = playerWeapon.weaponPlayerSo.itemName + " Lvl " + playerWeapon.upgradeIndex + 1;
                        itemDescription = playerWeapon.weaponPlayerSo.itemDescription;
                    }
                }
            }
            else
            {
                foreach (var playerPassive in _playerPassiveArray)
                {
                    if (playerPassive.passivePlayerSo == newItemArray[i])
                    {
                        itemName = playerPassive.passivePlayerSo.itemName + " Lvl " + playerPassive.upgradeIndex;
                        itemDescription = playerPassive.passivePlayerSo.itemName;
                    }
                }
            }

            _itemChoiceArray[i].SetupItem(itemName, itemDescription, newItemArray[i].itemSprite);

            int index = i;
            _itemChoiceArray[i].AddListenerButton(()=>
            {
                Debug.LogError(newItemArray[index].GetType());
                
                if (newItemArray[index].GetType() == typeof(WeaponPlayerSO))
                {
                    AddWeaponToArray((WeaponPlayerSO)newItemArray[index]);
                }
                else
                {
                    AddPassiveToArray((PassivePlayerSO)newItemArray[index]);
                }
                
                callbackClosePanel.Invoke();
            });
        }
    }

    public void ResetInventory()
    {
        
    }

    private bool CheckIfPassiveHasEmpty()
    {
        foreach (var passive in _playerPassiveArray)
        {
            if (passive.passivePlayerSo == null)
            {
                return true;
            }
        }

        return false;
    }
    
    private bool CheckIfWeaponHasEmpty()
    {
        foreach (var weapon in _playerWeaponArray)
        {
            if (weapon.weaponPlayerSo == null)
            {
                return true;
            }
        }

        return false;
    }

    #region PassiveBuff

    public int GetTotalMaxLife()
    {
        int totalMaxLife = 0;
        int maxlifeFromUpgrades = 0;
        foreach (var passive in _playerPassiveArray)
        {
            if (passive.passivePlayerSo != null)
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
            if (passive.passivePlayerSo != null)
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
        }


        return 1 + (totalDamage + damageFromUpgrades) / 100;
    }

    public float GetTotalPassiveFireRateMultiplier()
    {
        float fireRate = 0;
        float fireRateFromUpgrades = 0;
        foreach (var passive in _playerPassiveArray)
        {
            if (passive.passivePlayerSo != null)
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
        }

        return 1 - (fireRate + fireRateFromUpgrades)/100;
    }

    public float GetTotalPassiveProjectileSpeed()
    {
        float projectileSpeed = 0;
        float projectileSpeedFromUpgrades = 0;
        foreach (var passive in _playerPassiveArray)
        {
            if (passive.passivePlayerSo != null)
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
        }

        
        return 1 + (projectileSpeed + projectileSpeedFromUpgrades)/100;
    }
    
    public float GetTotalPassiveProjectileScale()
    {
        float projectileScale = 0;
        float projectileScaleFromUpgrades = 0;
        foreach (var passive in _playerPassiveArray)
        {
            if (passive.passivePlayerSo != null)
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
        }
        
        return 1 + (projectileScale + projectileScaleFromUpgrades)/100;
    }

    #endregion
  

    public PlayerWeapon[] PlayerWeaponArray => _playerWeaponArray;

    public PlayerPassive[] PlayerPassiveArray => _playerPassiveArray;
}