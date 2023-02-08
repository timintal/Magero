using System;
using System.Collections.Generic;
using _Game.Data;
using JetBrains.Annotations;
using Magero.UIFramework.Components.NavBar;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MageScreen : NavBarScreen
{
    [SerializeField] private WeaponSlot _leftHandWeapon;
    [SerializeField] private WeaponSlot _rightHandWeapon;

    [SerializeField] WeaponSlot _selectedWeapon;
    [SerializeField] WeaponUpgradePanel _upgradePanel;
    
    [SerializeField] Button _equipLeftHandButton;
    [SerializeField] Button _equipRightHandButton;
    
    [SerializeField] private WeaponSlot _slotTemplate;

    private List<WeaponSlot> _weaponSlots = new();

    private WeaponControlService _weaponControlService;
    private PlayerData _playerData;
    private GameSetup _gameSetup;
    
    [Inject, UsedImplicitly]
    public void SetDependencies(WeaponControlService controlService, PlayerData playerData,
        GameSetup gameSetup)
    {
        _weaponControlService = controlService;
        _playerData = playerData;
        _gameSetup = gameSetup;
    }

    private void OnEnable()
    {
        _equipLeftHandButton.onClick.AddListener(() =>
        {
            _weaponControlService.EquipWeapon(_selectedWeapon.WeaponType, true);
            UpdateSelectedWeapon();
        });
        
        _equipRightHandButton.onClick.AddListener(() =>
        {
            _weaponControlService.EquipWeapon(_selectedWeapon.WeaponType, false);
            UpdateSelectedWeapon();
        });
    }
    
    void OnDisable()
    {
        _equipLeftHandButton.onClick.RemoveAllListeners();
        _equipRightHandButton.onClick.RemoveAllListeners();
    }

    protected override void OnOpening()
    {
        UpdateSelectedWeapon();

        CreateWeaponSlots();
        
    }

    private void UpdateSelectedWeapon()
    {
        var leftSettings = _gameSetup.GetWeaponSettings(_playerData.LeftHandWeapon);
        var rightSettings = _gameSetup.GetWeaponSettings(_playerData.RightHandWeapon);

        if (leftSettings != null)
        {
            _leftHandWeapon.Init(leftSettings,
                () =>
                {
                    _playerData.LeftHandWeapon = WeaponType.None;
                    UpdateSelectedWeapon();
                });
        }
        else
        {
            _leftHandWeapon.Clear();
        }

        if (rightSettings != null)
        {
            _rightHandWeapon.Init(rightSettings,
                () =>
                {
                    _playerData.RightHandWeapon = WeaponType.None;
                    UpdateSelectedWeapon();
                });
        }
        else
        {
            _rightHandWeapon.Clear();
        }
    }

    protected override void OnClosed()
    {
        foreach (var weaponSlot in _weaponSlots)
        {
            Destroy(weaponSlot.gameObject);
        }

        _weaponSlots.Clear();
    }

    void CreateWeaponSlots()
    {
        foreach (var weaponSetting in _gameSetup.WeaponSettings)
        {
            var weaponSlot = Instantiate(_slotTemplate, _slotTemplate.transform.parent);
            weaponSlot.gameObject.SetActive(true);
            weaponSlot.Init(weaponSetting, () =>
            {
                RemoveSelectionFromAll();

                weaponSlot.SetSelected(true);
                _selectedWeapon.Init(weaponSetting, null);
                _upgradePanel.Init(weaponSetting);
                
                UpdateSelectedWeapon();
            });
            
            _weaponSlots.Add(weaponSlot);
        }
    }
    
    void RemoveSelectionFromAll()
    {
        foreach (var weaponSlot in _weaponSlots)
        {
            weaponSlot.SetSelected(false);
        }
    }
}