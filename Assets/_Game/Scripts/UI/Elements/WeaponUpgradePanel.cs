using System.Collections.Generic;
using _Game.Data;
using Game.Config.Model;
using UnityEngine;
using VContainer;

public class WeaponUpgradePanel : MonoBehaviour
{
    [SerializeField] UpgradableParamView _upgradableParamViewTemplate;

    List<UpgradableParamView> _upgradableParamViews = new List<UpgradableParamView>();

    [Inject] private WeaponData _weaponData;
    [Inject] GameConfig _gameConfig;

    public void Init(WeaponSettings settings)
    {
        Clear();
        for (var i = 0; i < settings.UpgradableParams.Count; i++)
        {
            var upgradeParam = settings.UpgradableParams[i];
            UpgradableParamView view;
            if (_upgradableParamViews.Count > i)
            {
                view = _upgradableParamViews[i];
            }
            else
            {
                view = Instantiate(_upgradableParamViewTemplate, _upgradableParamViewTemplate.transform.parent);
                _upgradableParamViews.Add(view);
            }

            view.gameObject.SetActive(true);

            SetupView(settings, upgradeParam, view);
        }
    }

    private void SetupView(WeaponSettings settings, UpgradableWeaponParam upgradeParam, UpgradableParamView view)
    {
        var currLevel = _weaponData.GetWeaponParamLevel(settings.Type, upgradeParam.ParamKey);
        var paramValueForLevel = upgradeParam.GetParamValueForLevel(currLevel + 1, _gameConfig);
        if (paramValueForLevel > 0)
        {
            view.Init(upgradeParam.ParamName,
                upgradeParam.GetParamValueForLevel(currLevel, _gameConfig).ToString("F1"),
                paramValueForLevel.ToString("F1"),
                upgradeParam.GetParamUpgradePriceForLevel(currLevel, _gameConfig),
                () =>
                {
                    _weaponData.IncreaseWeaponParamLevel(settings.Type, upgradeParam.ParamKey);
                    SetupView(settings, upgradeParam, view);
                });
        }
        else
        {
            view.Init(upgradeParam.ParamName,
                string.Empty,
                string.Empty,
                0,
                null, true);
        }
    }

    void Clear()
    {
        foreach (var param in _upgradableParamViews)
        {
            param.gameObject.SetActive(false);
        }
    }
}