using System.Collections.Generic;
using _Game.Data;
using Game.Config.Model;
using UnityEngine;

[CreateAssetMenu(menuName = "Magero/Weapons/Black Hole Settings")]
public class BlackHoleSettingsSO : WeaponSettings
{
    public override WeaponType Type => WeaponType.BlackHole;
    
    public float Cooldown;
    public float ExplosionRadius;
    public float PullSpeed;
    public float ProjectileSpeed;
    public GameObject ProjectilePrefab;
    public GameObject BlackHolePrefab;
    public GameObject ExplosionPrefab;
    private List<UpgradableWeaponParam> _upgradableParams;

    public override void ConfigWeaponEntity(GameEntity entity, GameSceneReferences sceneReferences, int armIndex, WeaponData weaponData, GameConfig gameConfig)
    {
        int sizeLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.HoleSize));
        int durationLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.HoleDuration));
        int damageLevel = weaponData.GetWeaponParamLevel(Type, nameof(SpellsStatsModel.HoleDamage));
        
        entity.AddProjectileShooter(Cooldown,
            ProjectilePrefab,
            ProjectileSpeed);
            
        entity.AddBlackHoleShooter(ExplosionRadius, 
            PullSpeed, 
            gameConfig.GetConfigModel<SpellsStatsModel>()[sizeLevel.ToString()].HoleSize, 
            gameConfig.GetConfigModel<SpellsStatsModel>()[durationLevel.ToString()].HoleDuration, 
            BlackHolePrefab,
            ExplosionPrefab);
            
        entity.AddTransform(sceneReferences.Arms[armIndex].ProjectileShootingTransform);
        entity.AddAnimator(sceneReferences.Arms[armIndex].Animator);
            
        entity.AddDamage(gameConfig.GetConfigModel<SpellsStatsModel>()[damageLevel.ToString()].HoleDamage);
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
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.HoleDamage, ParamName = "Damage", ParamKey = nameof(SpellsStatsModel.HoleDamage)});
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.HoleSize, ParamName = "Size", ParamKey = nameof(SpellsStatsModel.HoleSize)});
                _upgradableParams.Add(new UpgradableWeaponParam{GetParamValue = model => model.HoleDuration, ParamName = "Duration", ParamKey = nameof(SpellsStatsModel.HoleDuration)});
            }

            return _upgradableParams;
        }
    }
}