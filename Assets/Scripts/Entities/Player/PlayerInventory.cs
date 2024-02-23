using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
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
        if (upgradeIndex != 0 && upgradeIndex < 4)
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
        if (upgradeIndex != 0 && upgradeIndex > 4)
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
    private WeaponPlayerSO _baseWeaponPlayer;
    [SerializeField]
    private PlayerWeapon[] _playerWeaponArray;
    [SerializeField] 
    private PlayerPassive[] _playerPassiveArray;
    
    private int _indexWeaponInventory;
    private int _indexPassiveInventory;

    [Header("XP Bonus")]
    [SerializeField]
    private List<ItemPlayerSO> _itemGame = new List<ItemPlayerSO>();
    [SerializeField]
    private ItemChoice[] _itemChoiceArray = new ItemChoice[3];

    private void Start()
    {
        AddWeaponToArray(_baseWeaponPlayer);
    }

    public void AddWeaponToArray(WeaponPlayerSO weaponSO)
    {
        PlayerWeapon playerWeapon = _playerWeaponArray.ToList().Find(weapon => weaponSO == weapon.weaponPlayerSo);
        if (playerWeapon != null)
        {
            // Upgrade
            playerWeapon.upgradeIndex++;
            
            //Check fully upgraded
            if (playerWeapon.upgradeIndex >= playerWeapon.weaponPlayerSo.weaponLevelArray.Length)
            {
                _itemGame.Remove(playerWeapon.weaponPlayerSo);
                if (_itemGame.Count == 0)
                {
                    return;
                }
            }
        }
        else
        {
            // Add new
            if (CheckIfWeaponHasEmpty())
            {
                _playerWeaponArray[_indexWeaponInventory] = new PlayerWeapon(weaponSO);

                _indexWeaponInventory++;
            }
        }
        
        if (!CheckIfWeaponHasEmpty())
        {
            List<WeaponPlayerSO> listPlayerWeapon = GetPlayerWeapons();
            List<WeaponPlayerSO> listWeapon = new List<WeaponPlayerSO>();

            foreach (var item in _itemGame)
            {
                if (item.GetType() == typeof(WeaponPlayerSO))
                {
                    listWeapon.Add((WeaponPlayerSO) item);
                }
            }

            foreach (var weaponPlayer in listPlayerWeapon)
            {
                listWeapon.Remove(weaponPlayer);
            }
            
            foreach (var weapon in listWeapon)
            {
                _itemGame.Remove(weapon);
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
            if (playerPassive.upgradeIndex >= playerPassive.passivePlayerSo.passiveLevelArray.Length)
            {
                _itemGame.Remove(playerPassive.passivePlayerSo);
                if (_itemGame.Count == 0)
                {
                    return;
                }
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
            
        }

        if (!CheckIfPassiveHasEmpty())
        {
            List<PassivePlayerSO> listPlayerPassives = GetPlayerPassives();
            List<PassivePlayerSO> listPassives = new List<PassivePlayerSO>();

            foreach (var item in _itemGame)
            {
                if (item.GetType() == typeof(PassivePlayerSO))
                {
                    listPassives.Add((PassivePlayerSO) item);
                }
            }

            foreach (var passivePlayer in listPlayerPassives)
            {
                listPassives.Remove(passivePlayer);
            }
            
            foreach (var passive in listPassives)
            {
                _itemGame.Remove(passive);
            }
        }
    }

    public void DisplayUpgrades(UnityAction callbackClosePanel)
    {
        List<ItemPlayerSO> itemArray = new List<ItemPlayerSO>(_itemGame);

        int newItemArrayCount = itemArray.Count >= 3 ? 3 : itemArray.Count;
        ItemPlayerSO[] newItemArray = new ItemPlayerSO[newItemArrayCount];

        for (int i = 0; i < newItemArray.Length; i++)
        {
            ItemPlayerSO itemPlayer = itemArray[Random.Range(0, itemArray.Count)];
            newItemArray[i] = itemPlayer;
            itemArray.Remove(itemPlayer);
        }
        
        for (int i = 0; i < _itemChoiceArray.Length; i++)
        {
            if ((i + 1) > newItemArray.Length)
            {
                for (int j = i; j < _itemChoiceArray.Length; j++)
                {
                    _itemChoiceArray[j].gameObject.SetActive(false);
                }

                return;
            }
            
            _itemChoiceArray[i].gameObject.SetActive(true);
            
            string itemName = newItemArray[i].itemName;
            string itemDescription = newItemArray[i].itemDescription;

            //Check if item already in array, then 
            if (newItemArray[i].GetType() == typeof(WeaponPlayerSO))
            {
                foreach (var playerWeapon in _playerWeaponArray)
                {
                    if (playerWeapon.weaponPlayerSo == newItemArray[i])
                    {
                        itemName = playerWeapon.weaponPlayerSo.itemName + " Lvl " + (playerWeapon.upgradeIndex + 1);
                        if(playerWeapon.upgradeIndex < playerWeapon.weaponPlayerSo.weaponLevelArray.Length)
                        {
                            itemDescription = playerWeapon.weaponPlayerSo.weaponLevelArray[playerWeapon.upgradeIndex].upgradeDescription;
                        }
                    }
                }
            }
            else
            {
                foreach (var playerPassive in _playerPassiveArray)
                {
                    if (playerPassive.passivePlayerSo == newItemArray[i])
                    {
                        itemName = playerPassive.passivePlayerSo.itemName + " Lvl " + (playerPassive.upgradeIndex + 1);
                        itemDescription = playerPassive.passivePlayerSo.passiveLevelArray[playerPassive.upgradeIndex].upgradeDescription;
                    }
                }
            }

            _itemChoiceArray[i].SetupItem(itemName, itemDescription, newItemArray[i].itemSprite);

            int index = i;
            _itemChoiceArray[i].AddListenerButton(()=>
            {
                if (newItemArray[index].GetType() == typeof(WeaponPlayerSO))
                {
                    AddWeaponToArray((WeaponPlayerSO)newItemArray[index]);
                }
                else
                {
                    AddPassiveToArray((PassivePlayerSO)newItemArray[index]);
                }
                GameManager.Instance.SetGameState(GameState.Game);
                callbackClosePanel.Invoke();
            });
        }
    }

    public bool IsItemGameEmpty()
    {
        return _itemGame.Count <= 0;
    }

    private List<WeaponPlayerSO> GetPlayerWeapons()
    {
        List<WeaponPlayerSO> returnList = new List<WeaponPlayerSO>();
        foreach (var weapon in _playerWeaponArray)
        {
            returnList.Add(weapon.weaponPlayerSo);
        }

        return returnList;
    }

    private List<PassivePlayerSO> GetPlayerPassives()
    {
        List<PassivePlayerSO> returnList = new List<PassivePlayerSO>();
        foreach (var passive in _playerPassiveArray)
        {
            returnList.Add(passive.passivePlayerSo);
        }

        return returnList;
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
            if (passive.passivePlayerSo != null)
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