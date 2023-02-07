using _Game.Data;
using UnityEngine;

[CreateAssetMenu(menuName = "Magero/Weapons/Black Hole Settings")]
public class BlackHoleSettingsSO : WeaponSettings
{
    public override WeaponType Type => WeaponType.BlackHole;
    
    public float Cooldown;
    public float Damage;
    public float ExplosionRadius;
    public float PullSpeed;
    public float PullRadius;
    public float Lifetime;
    public float ProjectileSpeed;
    public GameObject ProjectilePrefab;
    public GameObject BlackHolePrefab;
    public GameObject ExplosionPrefab;

    public override void ConfigWeaponEntity(GameEntity entity, GameSceneReferences sceneReferences, int armIndex)
    {
        entity.AddProjectileShooter(Cooldown,
            ProjectilePrefab,
            ProjectileSpeed);
            
        entity.AddBlackHoleShooter(ExplosionRadius, 
            PullSpeed, 
            PullRadius, 
            Lifetime, 
            BlackHolePrefab,
            ExplosionPrefab);
            
        entity.AddTransform(sceneReferences.Arms[armIndex].ProjectileShootingTransform);
        entity.AddAnimator(sceneReferences.Arms[armIndex].Animator);
            
        entity.AddDamage(Damage);
        entity.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));

        entity.isPlayerWeaponDirection = true;
        entity.isPlayer = true;
    }
}