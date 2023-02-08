using System.Collections.Generic;
using _Game.Data;
using Game.Config.Model;
using UnityEngine;

[CreateAssetMenu(menuName = "Magero/Weapons/Gas Cloud Settings")]
public class GasCloudSettingsSO : WeaponSettings
{
    public override WeaponType Type => WeaponType.GasCloud;
    
    public float Cooldown;
    public GameObject ProjectilePrefab;
    public GameObject CloudPrefab;
    public float ProjectileSpeed;
    public float CloudSpeedMultiplier;
    private List<UpgradableWeaponParam> _upgradableParams;


    public override void ConfigWeaponEntity(GameEntity entity, GameSceneReferences sceneReferences, int armIndex, WeaponData weaponData, GameConfig gameConfig)
    {
        int damageLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.GasDps));
        int durationLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.GasDuration));
        int sizeLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.GasSize));
        
        entity.AddProjectileShooter(Cooldown,
            ProjectilePrefab,
            ProjectileSpeed);
            
        entity.AddGasProjectileShooter(gameConfig.GetConfigModel<SpellsStatsModel>()[sizeLevel.ToString()].GasSize,
            CloudSpeedMultiplier, 
            CloudPrefab,
            gameConfig.GetConfigModel<SpellsStatsModel>()[durationLevel.ToString()].GasDuration);
            
        entity.AddTransform(sceneReferences.Arms[armIndex].ProjectileShootingTransform);
        entity.AddAnimator(sceneReferences.Arms[armIndex].Animator);
        entity.AddDamage(gameConfig.GetConfigModel<SpellsStatsModel>()[damageLevel.ToString()].GasDps);
        entity.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));

        entity.isPlayerWeaponDirection = true;
        entity.isPlayer = true;
    }
    
    public override List<UpgradableWeaponParam> UpgradableParams
    {
        get
        {
            if (_upgradableParams == null)
            {
                _upgradableParams = new List<UpgradableWeaponParam>();
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.GasDps, 
                    ParamName = "Damage",
                    ParamKey = nameof(SpellsStatsModel.GasDps),
                    GetParamUpgradePrice = model => model.GasDps
                });
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.GasDuration, 
                    ParamName = "Duration",
                    ParamKey = nameof(SpellsStatsModel.GasDuration),
                    GetParamUpgradePrice = model => model.GasDuration
                });
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.GasSize, 
                    ParamName = "Radius",
                    ParamKey = nameof(SpellsStatsModel.GasSize),
                    GetParamUpgradePrice = model => model.GasSize
                });
            }

            return _upgradableParams;
        }
    }
}