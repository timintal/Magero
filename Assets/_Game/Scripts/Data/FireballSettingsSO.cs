using _Game.Data;
using UnityEngine;

[CreateAssetMenu(menuName = "Magero/Weapons/Fireball Settings")]
public class FireballSettingsSO : WeaponSettings
{
    public override WeaponType Type => WeaponType.Fireball;
    
    public float Cooldown;
    public float Damage;
    public GameObject ProjectilePrefab;
    public GameObject ExplosionVisualPrefab;
    public float ProjectileSpeed;
    public float ExplosionRadius;

    public override void ConfigWeaponEntity(GameEntity entity,GameSceneReferences sceneReferences, int armIndex)
    {
        entity.AddProjectileShooter(Cooldown,
            ProjectilePrefab,
            ProjectileSpeed);

        entity.AddExplodableProjectileShooter(ExplosionRadius, ExplosionVisualPrefab);
        entity.AddTransform(sceneReferences.Arms[armIndex].ProjectileShootingTransform);
        entity.AddAnimator(sceneReferences.Arms[armIndex].Animator);
        entity.AddDamage(Damage);
        entity.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy", "Environment"));
        entity.isPlayerWeaponDirection = true;
        entity.isPlayer = true;
    }
}