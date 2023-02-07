using _Game.Data;
using UnityEngine;

[CreateAssetMenu(menuName = "Magero/Weapons/Laser Settings")]
public class LaserSettingsSO : WeaponSettings
{
    public override WeaponType Type => WeaponType.Laser;
    public float Damage;

    public override void ConfigWeaponEntity(GameEntity entity,GameSceneReferences sceneReferences, int armIndex)
    {
        entity.isLaserShooter = true;
        entity.AddTransform(sceneReferences.Arms[armIndex].BeamShootingTransform);
        entity.AddHitPointEffect(sceneReferences.LaserSparkles);
        entity.AddBeamRenderer(sceneReferences.LaserRenderer);
        entity.AddWeaponHitPoint(sceneReferences.Arms[armIndex].BeamShootingTransform.position);
        entity.AddAnimator(sceneReferences.Arms[armIndex].Animator);
        entity.AddDamage(Damage);
        entity.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));
        entity.isPlayerWeaponDirection = true;
        entity.isPlayer = true;
    }
}