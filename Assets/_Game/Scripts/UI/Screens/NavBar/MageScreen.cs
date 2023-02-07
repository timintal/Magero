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

    [SerializeField] private WeaponSlot _slotTemplate;
    [SerializeField] private Button _upgradeLeftButton;
    [SerializeField] private Button _upgradeRightButton;

    private List<WeaponSlot> _weaponSlots = new();

    private WeaponControlService _weaponControlService;
    private PlayerData _playerData;
    private WeaponData _weaponData;
    private GameSetup _gameSetup;
    
    [Inject, UsedImplicitly]
    public void SetDependencies(WeaponControlService controlService, PlayerData playerData, WeaponData weaponData,
        GameSetup gameSetup)
    {
        _weaponControlService = controlService;
        _playerData = playerData;
        _weaponData = weaponData;
        _gameSetup = gameSetup;
    }

    void OnEnable()
    {
        _upgradeLeftButton.onClick.AddListener(() =>
        {
            _weaponData.UpgradeWeaponLevel(_playerData.LeftHandWeapon);
            UpdateSelectedWeapon();
        });
        _upgradeRightButton.onClick.AddListener(() =>
        {
            _weaponData.UpgradeWeaponLevel(_playerData.RightHandWeapon);
            UpdateSelectedWeapon();
        });
    }

    void OnDisable()
    {
        _upgradeLeftButton.onClick.RemoveAllListeners();
        _upgradeRightButton.onClick.RemoveAllListeners();
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
            _leftHandWeapon.DeInit();
            _leftHandWeapon.Init(leftSettings, _weaponData,
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
            _rightHandWeapon.DeInit();
            _rightHandWeapon.Init(rightSettings, _weaponData,
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
            weaponSlot.DeInit();
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
            weaponSlot.Init(weaponSetting, _weaponData, () =>
            {
                _weaponControlService.EquipWeapon(weaponSetting.Type);
                UpdateSelectedWeapon();
            });
            
            _weaponSlots.Add(weaponSlot);
        }
    }
}