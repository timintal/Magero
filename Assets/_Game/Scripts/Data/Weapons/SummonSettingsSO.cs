using _Game.Data;
using UnityEngine;

[CreateAssetMenu(menuName = "Magero/Weapons/Summon Settings")]
public class SummonSettingsSO : WeaponSettings
{
    public override WeaponType Type => WeaponType.Summon;
    
    public float Cooldown;
    public int Count;
    public float UnitDamage;
    public float UnitSpeed;
    public float UnitRadius;
    public float Lifetime;
    public GameObject SummonPrefab;
    public GameObject ExplosionPrefab;

    public override void ConfigWeaponEntity(GameEntity entity, GameSceneReferences sceneReferences, int armIndex)
    {
        entity.AddSummonSpell(Cooldown, 
            Count, 
            UnitSpeed, 
            UnitRadius, 
            Lifetime,
            ExplosionPrefab);
            
        entity.AddAssetLink(SummonPrefab);

        sceneReferences.Arms[armIndex].transform.position = sceneReferences.Arms[armIndex].transform.position +
                                                            Vector3.up * 0.02f + 
                                                            Vector3.left * sceneReferences.Arms[armIndex].transform.localScale.x * 0.03f;
            
        entity.AddTransform(sceneReferences.Arms[armIndex].BeamShootingTransform);
        entity.AddAnimator(sceneReferences.Arms[armIndex].Animator);
        entity.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));
        entity.AddTarget(TargetType.Player);
        entity.AddDamage(UnitDamage);

        entity.isPlayerWeaponDirection = true;
        entity.isPlayer = true;
    }
}