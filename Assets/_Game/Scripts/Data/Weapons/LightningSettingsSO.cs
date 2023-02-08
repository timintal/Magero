using System.Collections.Generic;
using _Game.Data;
using Game.Config.Model;
using UnityEngine;

[CreateAssetMenu(menuName = "Magero/Weapons/Lightning Settings")]
public class LightningSettingsSO : WeaponSettings
{
    public override WeaponType Type => WeaponType.Lightning;
    
    public float Cooldown;
    public float AOEDamageFactor;
    public GameObject ImpactFx;
    private List<UpgradableWeaponParam> _upgradableParams;

    public override void ConfigWeaponEntity(GameEntity entity, GameSceneReferences sceneReferences, int armIndex, WeaponData weaponData, GameConfig gameConfig)
    {
        int damageLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.LightningDamage));
        int sizeLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.LightningSize));
        int durationLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.LightningDuration));
        
        entity.AddLightningShooter(
            Cooldown,
            gameConfig.GetConfigModel<SpellsStatsModel>()[sizeLevel.ToString()].LightningSize,
            gameConfig.GetConfigModel<SpellsStatsModel>()[damageLevel.ToString()].LightningDamage,
            gameConfig.GetConfigModel<SpellsStatsModel>()[damageLevel.ToString()].LightningDamage * AOEDamageFactor,
            gameConfig.GetConfigModel<SpellsStatsModel>()[durationLevel.ToString()].LightningDuration);

        sceneReferences.Arms[armIndex].transform.position = sceneReferences.Arms[armIndex].transform.position +
                                                            Vector3.up * 0.02f + 
                                                            Vector3.left * sceneReferences.Arms[armIndex].transform.localScale.x * 0.03f;

            
        entity.AddTransform(sceneReferences.Arms[armIndex].BeamShootingTransform);
        entity.AddAnimator(sceneReferences.Arms[armIndex].Animator);
        entity.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));
        entity.AddAssetLink(ImpactFx);

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
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.LightningDamage, 
                    ParamName = "Damage", 
                    ParamKey = nameof(SpellsStatsModel.LightningDamage),    
                    GetParamUpgradePrice = model => model.LightningDamage
                });
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.LightningDuration, 
                    ParamName = "Stun Duration",
                    ParamKey = nameof(SpellsStatsModel.LightningDuration),
                    GetParamUpgradePrice = model => model.LightningDuration
                });
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.LightningSize,
                    ParamName = "Radius",
                    ParamKey = nameof(SpellsStatsModel.LightningSize),
                    GetParamUpgradePrice = model => model.LightningSize
                });
            }

            return _upgradableParams;
        }
    }
}