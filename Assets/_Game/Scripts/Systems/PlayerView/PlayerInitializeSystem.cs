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

        if (gameSetup.AddFireballs)
        {
            var fireballShooter = _contexts.game.CreateEntity();

            fireballShooter.AddProjectileShooter(gameSetup.FireballSetings.Cooldown,
                gameSetup.FireballSetings.ProjectilePrefab,
                gameSetup.FireballSetings.ProjectileSpeed);

            fireballShooter.AddExplodableProjectileShooter(gameSetup.FireballSetings.ExplosionRadius);
            fireballShooter.AddTransform(sceneReferences.FireballsShootTransform);
            fireballShooter.AddAnimator(sceneReferences.FireballAnimator);
            fireballShooter.AddDamage(gameSetup.FireballSetings.Damage);
            fireballShooter.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy", "Environment"));
            fireballShooter.isPlayerWeaponDirection = true;
            fireballShooter.isPlayer = true;
        }

        if (gameSetup.AddLaser)
        {
            var laserEntity = _contexts.game.CreateEntity();
            laserEntity.isLaserShooter = true;
            laserEntity.AddTransform(sceneReferences.LaserShootTransform);
            laserEntity.AddHitPointEffect(sceneReferences.LaserSparkles);
            laserEntity.AddBeamRenderer(sceneReferences.LaserRenderer);
            laserEntity.AddWeaponHitPoint(sceneReferences.LaserShootTransform.position);
            laserEntity.AddAnimator(sceneReferences.LaserAnimator);
            laserEntity.AddDamage(gameSetup.LaserSettings.DamagePerSecond);
            laserEntity.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));
            laserEntity.isPlayerWeaponDirection = true;
            laserEntity.isPlayer = true;
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

            lightningShooter.AddTransform(sceneReferences.LaserShootTransform);
            lightningShooter.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));

            lightningShooter.isPlayerWeaponDirection = true;
            lightningShooter.isPlayer = true;
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

            acidSpray.AddDamage(gameSetup.AcidStreamSettings.DamagePerSecond);
            acidSpray.AddTransform(sceneReferences.LaserShootTransform);
            acidSpray.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));

            acidSpray.isPlayerWeaponDirection = true;
            acidSpray.isPlayer = true;
        }

        if (gameSetup.AddGasCloud)
        {
            var gasShooter = _contexts.game.CreateEntity();

            gasShooter.AddProjectileShooter(gameSetup.GasCloudSettings.Cooldown,
                gameSetup.GasCloudSettings.ProjectilePrefab,
                gameSetup.GasCloudSettings.ProjectileSpeed);

            gasShooter.AddGasProjectileShooter(gameSetup.GasCloudSettings.CloudRadius, gameSetup.GasCloudSettings.CloudSpeedMultiplier, gameSetup.GasCloudSettings.CloudPrefab);
            gasShooter.AddTransform(sceneReferences.FireballsShootTransform);
            gasShooter.AddAnimator(sceneReferences.FireballAnimator);
            gasShooter.AddDamage(gameSetup.GasCloudSettings.Damage);
            gasShooter.AddAttacker(TargetType.Enemy, LayerMask.GetMask("Enemy"));

            gasShooter.isPlayerWeaponDirection = true;
            gasShooter.isPlayer = true;
        }
    }
}