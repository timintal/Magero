using System.Collections.Generic;
using _Game.Data;
using Game.Config.Model;
using UnityEngine;

[CreateAssetMenu(menuName = "Magero/Weapons/Summon Settings")]
public class SummonSettingsSO : WeaponSettings
{
    public override WeaponType Type => WeaponType.Summon;
    
    public float Cooldown;
    public int Count;
    public float UnitRadius;
    public GameObject SummonPrefab;
    public GameObject ExplosionPrefab;
    private List<UpgradableWeaponParam> _upgradableParams;

    public override void ConfigWeaponEntity(GameEntity entity, GameSceneReferences sceneReferences, int armIndex, WeaponData weaponData, GameConfig gameConfig)
    {
        int damageLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.SummonDamage));
        int durationLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.SummonDuration));
        int speedLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.SummonSpeed));
        
        entity.AddSummonSpell(Cooldown, 
            Count, 
            gameConfig.GetConfigModel<SpellsStatsModel>()[speedLevel.ToString()].SummonSpeed, 
            UnitRadius, 
            gameConfig.GetConfigModel<SpellsStatsModel>()[durationLevel.ToString()].SummonDuration,
            ExplosionPrefab);
            
        entity.AddAssetLink(SummonPrefab);

        sceneReferences.Arms[armIndex].transform.position = sceneReferences.Arms[armIndex].transform.position +
                                                            Vector3.up * 0.02f + 
                                                            Vector3.left * sceneReferences.Arms[armIndex].transform.localScale.x * 0.03f;
            
        entity.AddTransform(sceneReferences.Arms[armIndex].BeamShootingTransform);
        entity.AddAnimator(sceneReferences.Arms[armIndex].Animator);
        entity.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));
        entity.AddTarget(TargetType.Player);
        entity.AddDamage(gameConfig.GetConfigModel<SpellsStatsModel>()[damageLevel.ToString()].SummonDamage);

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
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.SummonDamage, 
                    ParamName = "Damage", 
                    ParamKey = nameof(SpellsStatsModel.SummonDamage),
                    GetParamUpgradePrice = model => model.SummonDamage
                });
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.SummonDuration,
                    ParamName = "Duration", 
                    ParamKey = nameof(SpellsStatsModel.SummonDuration),
                    GetParamUpgradePrice = model => model.SummonDuration
                });
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.SummonSpeed, 
                    ParamName = "Speed",
                    ParamKey = nameof(SpellsStatsModel.SummonSpeed),
                    GetParamUpgradePrice = model => model.SummonSpeed
                });
            }

            return _upgradableParams;
        }
    }
}