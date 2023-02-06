using _Game.Data;
using UnityEngine;

[CreateAssetMenu(menuName = "Magero/Weapons/Acid Settings")]
public class AcidSettingsSO : WeaponSettings
{
    public override WeaponType Type => WeaponType.Acid;
    
    public float Cooldown;
    public float PoolRadius;
    public GameObject PuddlePrefab;
    public float Damage;
    public float PuddleLifetime;
    public float RefreshTimestamp;
    public AnimationCurve RadiusCurve;


    public override void ConfigWeaponEntity(GameEntity entity, GameSceneReferences sceneReferences, int armIndex)
    {
        entity.AddAcidStream(
            Cooldown,
            PoolRadius,
            PuddlePrefab,
            PuddleLifetime,
            RefreshTimestamp,
            RadiusCurve);

        entity.AddBeamRenderer(sceneReferences.AcidRenderer);
        entity.AddWeaponHitPoint(sceneReferences.Arms[armIndex].BeamShootingTransform.position);
        entity.AddAnimator(sceneReferences.Arms[armIndex].Animator);

        entity.AddDamage(Damage);
        entity.AddTransform(sceneReferences.Arms[armIndex].BeamShootingTransform);
        entity.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));

        entity.isPlayerWeaponDirection = true;
        entity.isPlayer = true;
    }
}