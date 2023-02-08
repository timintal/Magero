using System.Collections.Generic;
using _Game.Data;
using Game.Config.Model;
using UnityEngine;

[CreateAssetMenu(menuName = "Magero/Weapons/Fireball Settings")]
public class FireballSettingsSO : WeaponSettings
{
    public override WeaponType Type => WeaponType.Fireball;
    
    public float Cooldown;
    public GameObject ProjectilePrefab;
    public GameObject ExplosionVisualPrefab;
    public float ProjectileSpeed;
    private List<UpgradableWeaponParam> _upgradableParams;

    public override void ConfigWeaponEntity(GameEntity entity,GameSceneReferences sceneReferences, int armIndex, WeaponData weaponData, GameConfig gameConfig)
    {
        int damageLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.FireballDamage));
        int sizeLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.FireballSize));
        
        entity.AddProjectileShooter(Cooldown,
            ProjectilePrefab,
            ProjectileSpeed);

        entity.AddExplodableProjectileShooter(
            gameConfig.GetConfigModel<SpellsStatsModel>()[sizeLevel.ToString()].FireballSize, 
            ExplosionVisualPrefab);
        
        entity.AddTransform(sceneReferences.Arms[armIndex].ProjectileShootingTransform);
        entity.AddAnimator(sceneReferences.Arms[armIndex].Animator);
        entity.AddDamage(gameConfig.GetConfigModel<SpellsStatsModel>()[damageLevel.ToString()].FireballDamage);
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
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.FireballDamage,
                    ParamName = "Damage",
                    ParamKey = nameof(SpellsStatsModel.FireballDamage),
                    GetParamUpgradePrice = model => model.FireballDamage
                });
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.FireballSize,
                    ParamName = "Radius", 
                    ParamKey = nameof(SpellsStatsModel.FireballSize),
                    GetParamUpgradePrice = model => model.FireballSize
                });
            }

            return _upgradableParams;
        }
    }
}