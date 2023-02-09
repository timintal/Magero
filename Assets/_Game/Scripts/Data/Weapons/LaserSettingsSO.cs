using System.Collections.Generic;
using _Game.Data;
using Game.Config.Model;
using UnityEngine;

[CreateAssetMenu(menuName = "Magero/Weapons/Laser Settings")]
public class LaserSettingsSO : WeaponSettings
{
    public override WeaponType Type => WeaponType.Laser;
    private List<UpgradableWeaponParam> _upgradableParams;

    public override void ConfigWeaponEntity(GameEntity entity,GameSceneReferences sceneReferences, int armIndex, WeaponData weaponData, GameConfig gameConfig)
    {
        int damageLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.LaserDps));
        entity.isLaserShooter = true;
        entity.AddTransform(sceneReferences.Arms[armIndex].BeamShootingTransform);
        entity.AddHitPointEffect(sceneReferences.LaserSparkles);
        entity.AddBeamRenderer(sceneReferences.LaserRenderer);
        entity.AddWeaponHitPoint(sceneReferences.Arms[armIndex].BeamShootingTransform.position);
        entity.AddAnimator(sceneReferences.Arms[armIndex].Animator);
        entity.AddDamage(gameConfig.GetConfigModel<SpellsStatsModel>()[damageLevel.ToString()].LaserDps);
        entity.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy", "Environment"));
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
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.LaserDps, 
                    ParamName = "Damage", 
                    ParamKey = nameof(SpellsStatsModel.LaserDps),
                    GetParamUpgradePrice = model => model.LaserDps
                });
            }

            return _upgradableParams;
        }
    }
}