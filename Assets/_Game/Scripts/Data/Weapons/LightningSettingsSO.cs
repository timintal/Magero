using _Game.Data;
using UnityEngine;

[CreateAssetMenu(menuName = "Magero/Weapons/Lightning Settings")]
public class LightningSettingsSO : WeaponSettings
{
    public override WeaponType Type => WeaponType.Lightning;
    
    public float Cooldown;
    public float EffectRadius;
    public int TargetDamage;
    public int AOEDamage;
    public float StunDuration;
    public GameObject ImpactFx;

    public override void ConfigWeaponEntity(GameEntity entity, GameSceneReferences sceneReferences, int armIndex)
    {
        entity.AddLightningShooter(
            Cooldown,
            EffectRadius,
            TargetDamage,
            AOEDamage,
            StunDuration);

        sceneReferences.Arms[armIndex].transform.position = sceneReferences.Arms[armIndex].transform.position +
                                                            Vector3.up * 0.02f + 
                                                            Vector3.left * sceneReferences.Arms[armIndex].transform.localScale.x * 0.03f;

            
        entity.AddTransform(sceneReferences.Arms[armIndex].BeamShootingTransform);
        entity.AddAnimator(sceneReferences.Arms[armIndex].Animator);
        entity.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));
        entity.AddAssetLink(ImpactFx);

        entity.isPlayerWeaponDirection = true;
        entity.isPlayer = true;
    }
}