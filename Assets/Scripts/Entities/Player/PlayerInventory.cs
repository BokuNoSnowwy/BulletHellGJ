using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] 
    private WeaponPlayerSO[] _playerWeaponArray;
    // [SerializeField] 
    // private PassivePlayerSO[] _playerPassiveArray;
    private int _indexWeaponInventory;
    private int _indexPassiveInventory;

    private PlayerShoot _playerShoot;

    private void Start()
    {
        _playerShoot = GetComponent<PlayerShoot>();
    }

    public void AddWeaponToArray(WeaponPlayerSO weaponSO)
    {
        if (_indexWeaponInventory < _playerWeaponArray.Length)
        {
            _playerWeaponArray[_indexWeaponInventory] = weaponSO;
        }
        else
        {
            Debug.LogError("Too much weapon yield ");
        }

        _indexWeaponInventory++;
    }

    public void ResetInventory()
    {
        
    }
}
