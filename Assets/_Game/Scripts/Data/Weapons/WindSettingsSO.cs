using System.Collections.Generic;
using _Game.Data;
using Game.Config.Model;
using UnityEngine;

[CreateAssetMenu(menuName = "Magero/Weapons/Wind Settings")]
public class WindSettingsSO : WeaponSettings
{
    public override WeaponType Type => WeaponType.Wind;
    
    public float PushDamping;
    public float WindStreamRadius;
    public float MaxDistance;
    
    private List<UpgradableWeaponParam> _upgradableParams;


    public override void ConfigWeaponEntity(GameEntity entity, GameSceneReferences sceneReferences, int armIndex, WeaponData weaponData, GameConfig gameConfig)
    {
        int damageLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.WindDps));
        int speedLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.WindSpeed));
        
        entity.AddWindBlower(gameConfig.GetConfigModel<SpellsStatsModel>()[speedLevel.ToString()].WindSpeed, PushDamping, MaxDistance);
        entity.AddRadius(WindStreamRadius);
            
        entity.AddTransform(sceneReferences.Arms[armIndex].BeamShootingTransform);

        entity.AddAnimator(sceneReferences.Arms[armIndex].Animator);
        entity.AddDamage(gameConfig.GetConfigModel<SpellsStatsModel>()[damageLevel.ToString()].WindDps);
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
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.WindDps, ParamName = "Damage", ParamKey = nameof(SpellsStatsModel.WindDps)});
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.WindSpeed, ParamName = "Speed", ParamKey = nameof(SpellsStatsModel.WindSpeed)});
                
            }

            return _upgradableParams;
        }
    }
}