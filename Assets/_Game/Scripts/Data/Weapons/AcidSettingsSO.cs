using System;
using System.Collections.Generic;
using _Game.Data;
using Game.Config.Model;
using UnityEngine;

[CreateAssetMenu(menuName = "Magero/Weapons/Acid Settings")]
public class AcidSettingsSO : WeaponSettings
{
    public override WeaponType Type => WeaponType.Acid;
    
    public float Cooldown;
    public GameObject PuddlePrefab;
    public float RefreshTimestamp;
    public AnimationCurve RadiusCurve;
    private List<UpgradableWeaponParam> _upgradableParams;


    public override void ConfigWeaponEntity(GameEntity entity, GameSceneReferences sceneReferences, int armIndex, WeaponData weaponData, GameConfig gameConfig)
    {
        int durationLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.AcidDuration));
        int sizeLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.AcidSize));
        var damageLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.AcidDps));
        
        entity.AddAcidStream(
            Cooldown,
            gameConfig.GetConfigModel<SpellsStatsModel>()[sizeLevel.ToString()].AcidSize,
            PuddlePrefab,
            gameConfig.GetConfigModel<SpellsStatsModel>()[durationLevel.ToString()].AcidDuration,
            RefreshTimestamp,
            RadiusCurve);

        entity.AddBeamRenderer(sceneReferences.AcidRenderer);
        entity.AddWeaponHitPoint(sceneReferences.Arms[armIndex].BeamShootingTransform.position);
        entity.AddAnimator(sceneReferences.Arms[armIndex].Animator);

        entity.AddDamage(gameConfig.GetConfigModel<SpellsStatsModel>()[damageLevel.ToString()].AcidDps);
        
        entity.AddTransform(sceneReferences.Arms[armIndex].BeamShootingTransform);
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
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.AcidDps, ParamName = "Damage", ParamKey = nameof(SpellsStatsModel.AcidDps)});
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.AcidDuration, ParamName = "Duration", ParamKey = nameof(SpellsStatsModel.AcidDuration)});
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.AcidSize, ParamName = "Size", ParamKey = nameof(SpellsStatsModel.AcidSize)});
            }

            return _upgradableParams;
        }
    }
}