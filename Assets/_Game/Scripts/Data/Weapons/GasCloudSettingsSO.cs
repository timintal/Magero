using _Game.Data;
using UnityEngine;

[CreateAssetMenu(menuName = "Magero/Weapons/Gas Cloud Settings")]
public class GasCloudSettingsSO : WeaponSettings
{
    public override WeaponType Type => WeaponType.GasCloud;
    
    public float Cooldown;
    public float Damage;
    public GameObject ProjectilePrefab;
    public GameObject CloudPrefab;
    public float ProjectileSpeed;
    public float CloudRadius;
    public float CloudSpeedMultiplier;
    public float CloudLifetime;


    public override void ConfigWeaponEntity(GameEntity entity, GameSceneReferences sceneReferences, int armIndex)
    {
        entity.AddProjectileShooter(Cooldown,
            ProjectilePrefab,
            ProjectileSpeed);
            
        entity.AddGasProjectileShooter(CloudRadius,
            CloudSpeedMultiplier, 
            CloudPrefab,
            CloudLifetime);
            
        entity.AddTransform(sceneReferences.Arms[armIndex].ProjectileShootingTransform);
        entity.AddAnimator(sceneReferences.Arms[armIndex].Animator);
        entity.AddDamage(Damage);
        entity.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));

        entity.isPlayerWeaponDirection = true;
        entity.isPlayer = true;
    }
}