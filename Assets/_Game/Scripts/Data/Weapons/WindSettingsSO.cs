using _Game.Data;
using UnityEngine;

[CreateAssetMenu(menuName = "Magero/Weapons/Wind Settings")]
public class WindSettingsSO : WeaponSettings
{
    public override WeaponType Type => WeaponType.Wind;
    
    public float Damage;
    public float PushSpeed;
    public float PushDamping;
    public float WindStreamRadius;
    public float MaxDistance;


    public override void ConfigWeaponEntity(GameEntity entity, GameSceneReferences sceneReferences, int armIndex)
    {
        entity.AddWindBlower(PushSpeed, PushDamping, MaxDistance);
        entity.AddRadius(WindStreamRadius);
            
        entity.AddTransform(sceneReferences.Arms[armIndex].BeamShootingTransform);

        entity.AddAnimator(sceneReferences.Arms[armIndex].Animator);
        entity.AddDamage(Damage);
        entity.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));
        entity.isPlayerWeaponDirection = true;
        entity.isPlayer = true;
    }
}