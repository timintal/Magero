using System;
using _Game.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField] private Image _weaponIcon;
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private TextMeshProUGUI _levelLabel;

    [SerializeField] private Button _selectionButton;

    private Action OnSelectedAction;
    private WeaponData _weaponData;
    private WeaponSettings _weaponSettings;

    public void Init(WeaponSettings settings, WeaponData weaponData, Action onSelected)
    {
        _weaponData = weaponData;
        _weaponSettings = settings;
        _weaponIcon.sprite = settings.WeaponSprite;
        _label.text = settings.WeaponName;

        OnSelectedAction = onSelected;
        var level = weaponData.GetWeaponLevel(settings.Type);
        _levelLabel.text = level > 0 ? $"Lv.{level}" : "";
        weaponData.OnWeaponLevelUpdated += OnWeaponLevelUpdated;
    }

    public void Clear()
    {
        OnSelectedAction = null;
        _weaponIcon.sprite = null;
        _label.text = "";
        _levelLabel.text = "";
    }

    public void DeInit()
    {
        if (_weaponData != null)
            _weaponData.OnWeaponLevelUpdated -= OnWeaponLevelUpdated;
    }

    private void OnWeaponLevelUpdated(WeaponType t, int lvl)
    {
        if (_weaponSettings.Type == t)
        {
            _levelLabel.text = $"Lv.{lvl}";
        }
    }

    void OnEnable()
    {
        _selectionButton.onClick.AddListener(() => OnSelectedAction?.Invoke());
    }

    void OnDisable()
    {
        _selectionButton.onClick.RemoveAllListeners();
    }
}