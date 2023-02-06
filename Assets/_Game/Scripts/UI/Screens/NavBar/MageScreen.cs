using System.Collections.Generic;
using _Game.Data;
using JetBrains.Annotations;
using Magero.UIFramework.Components.NavBar;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MageScreen : NavBarScreen
{
    [SerializeField] private Image _leftHandWeapon;
    [SerializeField] private Image _rightHandWeapon;

    [SerializeField] private Button _clearLeftHandButton;
    [SerializeField] private Button _clearRightHandButton;

    [SerializeField] private WeaponSlot _slotTemplate;

    private List<WeaponSlot> _weaponSlots = new();

    private WeaponControlService _weaponControlService;
    private PlayerData _playerData;
    private WeaponData _weaponData;
    private GameSetup _gameSetup;

    void OnEnable()
    {
        _clearLeftHandButton.onClick.AddListener(() =>
        {
            _playerData.LeftHandWeapon = WeaponType.None;
            UpdateSelectedWeapon();
        });
        _clearRightHandButton.onClick.AddListener(() =>
        {
            _playerData.RightHandWeapon = WeaponType.None;
            UpdateSelectedWeapon();
        });
    }

    void OnDisable()
    {
        _clearLeftHandButton.onClick.RemoveAllListeners();
        _clearRightHandButton.onClick.RemoveAllListeners();
    }

    [Inject, UsedImplicitly]
    public void SetDependencies(WeaponControlService controlService, PlayerData playerData, WeaponData weaponData,
        GameSetup gameSetup)
    {
        _weaponControlService = controlService;
        _playerData = playerData;
        _weaponData = weaponData;
        _gameSetup = gameSetup;
    }

    protected override void OnOpening()
    {
        UpdateSelectedWeapon();

        CreateWeaponSlots();
    }

    private void UpdateSelectedWeapon()
    {
        var leftHandWeapon = _gameSetup.GetWeaponSettings(_playerData.LeftHandWeapon);
        var rightHandWeapon = _gameSetup.GetWeaponSettings(_playerData.RightHandWeapon);

        if (leftHandWeapon != null)
        {
            _leftHandWeapon.sprite = leftHandWeapon.WeaponSprite;
        }
        else
        {
            _leftHandWeapon.sprite = null;
        }

        if (rightHandWeapon != null)
        {
            _rightHandWeapon.sprite = rightHandWeapon.WeaponSprite;
        }
        else
        {
            _rightHandWeapon.sprite = null;
        }
    }

    void CreateWeaponSlots()
    {
        foreach (var weaponSlot in _weaponSlots)
        {
            Destroy(weaponSlot.gameObject);
        }

        _weaponSlots.Clear();

        foreach (var weaponSetting in _gameSetup.WeaponSettings)
        {
            var weaponSlot = Instantiate(_slotTemplate, _slotTemplate.transform.parent);
            weaponSlot.gameObject.SetActive(true);
            weaponSlot.Init(weaponSetting.WeaponSprite, weaponSetting.WeaponName, () =>
            {
                _weaponControlService.EquipWeapon(weaponSetting.Type);
                UpdateSelectedWeapon();
            });
            _weaponSlots.Add(weaponSlot);
        }
    }
}