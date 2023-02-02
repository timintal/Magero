using Entitas;
using UnityEngine;

public class PlayerInitializeSystem : IInitializeSystem
{
    Contexts _contexts;

    public PlayerInitializeSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        var gameSetup = _contexts.game.gameSetup.value;
        var sceneReferences = _contexts.game.gameSceneReferences.value;

        var playerEntity = _contexts.game.CreateEntity();
        playerEntity.AddHealth(gameSetup.PlayerSettings.Health);
        playerEntity.AddMaxHealth(gameSetup.PlayerSettings.Health);
        playerEntity.AddTarget(TargetType.Player);
        playerEntity.AddTransform(sceneReferences.CameraTransform);
        playerEntity.AddHealthBarUI(sceneReferences.PlayerHealthBar);
        playerEntity.isPlayer = true;

        int armIndex = 0;
        
        if (gameSetup.AddFireballs)
        {
            var fireballShooter = _contexts.game.CreateEntity();

            fireballShooter.AddProjectileShooter(gameSetup.FireballSetings.Cooldown,
                gameSetup.FireballSetings.ProjectilePrefab,
                gameSetup.FireballSetings.ProjectileSpeed);

            fireballShooter.AddExplodableProjectileShooter(gameSetup.FireballSetings.ExplosionRadius);
            fireballShooter.AddTransform(sceneReferences.Arms[armIndex].ProjectileShootingTransform);
            fireballShooter.AddAnimator(sceneReferences.Arms[armIndex].Animator);
            fireballShooter.AddDamage(gameSetup.FireballSetings.Damage);
            fireballShooter.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy", "Environment"));
            fireballShooter.isPlayerWeaponDirection = true;
            fireballShooter.isPlayer = true;

            armIndex++;
        }

        if (gameSetup.AddLaser)
        {
            var laserEntity = _contexts.game.CreateEntity();
            laserEntity.isLaserShooter = true;
            laserEntity.AddTransform(sceneReferences.Arms[armIndex].BeamShootingTransform);
            laserEntity.AddHitPointEffect(sceneReferences.LaserSparkles);
            laserEntity.AddBeamRenderer(sceneReferences.LaserRenderer);
            laserEntity.AddWeaponHitPoint(sceneReferences.Arms[armIndex].BeamShootingTransform.position);
            laserEntity.AddAnimator(sceneReferences.Arms[armIndex].Animator);
            laserEntity.AddDamage(gameSetup.LaserSettings.DamagePerSecond);
            laserEntity.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));
            laserEntity.isPlayerWeaponDirection = true;
            laserEntity.isPlayer = true;

            armIndex++;
        }

        if (gameSetup.AddLightning)
        {
            var lightningShooter = _contexts.game.CreateEntity();
            lightningShooter.AddLightningShooter(
                gameSetup.LightningStrikeSettings.Cooldown,
                gameSetup.LightningStrikeSettings.EffectRadius,
                gameSetup.LightningStrikeSettings.TargetDamage,
                gameSetup.LightningStrikeSettings.AOEDamage,
                gameSetup.LightningStrikeSettings.StunDuration);

            lightningShooter.AddTransform(sceneReferences.Arms[armIndex].BeamShootingTransform);
            lightningShooter.AddAnimator(sceneReferences.Arms[armIndex].Animator);
            lightningShooter.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));

            lightningShooter.isPlayerWeaponDirection = true;
            lightningShooter.isPlayer = true;

            armIndex++;
        }

        if (gameSetup.AddAcid)
        {
            var acidSpray = _contexts.game.CreateEntity();
            acidSpray.AddAcidStream(
                gameSetup.AcidStreamSettings.Cooldown,
                gameSetup.AcidStreamSettings.PoolRadius,
                gameSetup.AcidStreamSettings.PuddlePrefab,
                gameSetup.AcidStreamSettings.PuddleLifetime,
                gameSetup.AcidStreamSettings.RefreshTimestamp,
                gameSetup.AcidStreamSettings.RadiusCurve);

            acidSpray.AddBeamRenderer(sceneReferences.AcidRenderer);
            acidSpray.AddWeaponHitPoint(sceneReferences.Arms[armIndex].BeamShootingTransform.position);
            acidSpray.AddAnimator(sceneReferences.Arms[armIndex].Animator);

            acidSpray.AddDamage(gameSetup.AcidStreamSettings.DamagePerSecond);
            acidSpray.AddTransform(sceneReferences.Arms[armIndex].BeamShootingTransform);
            acidSpray.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));

            acidSpray.isPlayerWeaponDirection = true;
            acidSpray.isPlayer = true;
            armIndex++;
        }

        if (gameSetup.AddGasCloud)
        {
            var gasShooter = _contexts.game.CreateEntity();

            gasShooter.AddProjectileShooter(gameSetup.GasCloudSettings.Cooldown,
                gameSetup.GasCloudSettings.ProjectilePrefab,
                gameSetup.GasCloudSettings.ProjectileSpeed);
            
            gasShooter.AddGasProjectileShooter(gameSetup.GasCloudSettings.CloudRadius,
                gameSetup.GasCloudSettings.CloudSpeedMultiplier, 
                gameSetup.GasCloudSettings.CloudPrefab,
                gameSetup.GasCloudSettings.CloudLifetime);
            
            gasShooter.AddTransform(sceneReferences.Arms[armIndex].ProjectileShootingTransform);
            gasShooter.AddAnimator(sceneReferences.Arms[armIndex].Animator);
            gasShooter.AddDamage(gameSetup.GasCloudSettings.Damage);
            gasShooter.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));

            gasShooter.isPlayerWeaponDirection = true;
            gasShooter.isPlayer = true;

            armIndex++;
        }

        if (gameSetup.AddWind)
        {
            var windBlower = _contexts.game.CreateEntity();
            windBlower.AddWindBlower(gameSetup.WindGustSettings.PushSpeed, gameSetup.WindGustSettings.PushDamping, gameSetup.WindGustSettings.MaxDistance);
            windBlower.AddRadius(gameSetup.WindGustSettings.WindStreamRadius);
            
            windBlower.AddTransform(sceneReferences.Arms[armIndex].BeamShootingTransform);

            windBlower.AddAnimator(sceneReferences.Arms[armIndex].Animator);
            windBlower.AddDamage(gameSetup.WindGustSettings.Damage);
            windBlower.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));
            windBlower.isPlayerWeaponDirection = true;
            windBlower.isPlayer = true;
            
            armIndex++;
        }

        if (gameSetup.AddBlackHole)
        {
            var gasShooter = _contexts.game.CreateEntity();

            gasShooter.AddProjectileShooter(gameSetup.BlackHoleSettings.Cooldown,
                gameSetup.BlackHoleSettings.ProjectilePrefab,
                gameSetup.BlackHoleSettings.ProjectileSpeed);
            
            gasShooter.AddBlackHoleShooter(gameSetup.BlackHoleSettings.ExplosionRadius, 
                gameSetup.BlackHoleSettings.PullSpeed, 
                gameSetup.BlackHoleSettings.PullRadius, 
                gameSetup.BlackHoleSettings.Lifetime, 
                gameSetup.BlackHoleSettings.BlackHolePrefab);
            
            gasShooter.AddTransform(sceneReferences.Arms[armIndex].ProjectileShootingTransform);
            gasShooter.AddAnimator(sceneReferences.Arms[armIndex].Animator);
            
            gasShooter.AddDamage(gameSetup.BlackHoleSettings.Damage);
            gasShooter.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));

            gasShooter.isPlayerWeaponDirection = true;
            gasShooter.isPlayer = true;

            armIndex++;
        }
        

        for (int i = armIndex; i < sceneReferences.Arms.Length; i++)
        {
            sceneReferences.Arms[i].gameObject.SetActive(false);
        }
    }
}