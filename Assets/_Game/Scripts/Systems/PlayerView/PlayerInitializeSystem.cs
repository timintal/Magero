using Entitas;

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
            var weaponEntity = _contexts.game.CreateEntity();

            weaponEntity.AddProjectileShooter(gameSetup.FireballSetings.Cooldown,
                gameSetup.FireballSetings.ProjectilePrefab,
                gameSetup.FireballSetings.ProjectileSpeed,
                TargetType.Enemy);

            weaponEntity.AddExplodableProjectileShooter(gameSetup.FireballSetings.ExplosionRadius);
            weaponEntity.AddTransform(sceneReferences.FireballsShootTransform);
            weaponEntity.AddAnimator(sceneReferences.FireballAnimator);
            weaponEntity.AddDamage(gameSetup.FireballSetings.Damage);
            weaponEntity.isPlayerWeaponDirection = true;
            weaponEntity.isPlayer = true;
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
                gameSetup.AcidStreamSettings.RadiusCurve);

            acidSpray.AddDamage(gameSetup.AcidStreamSettings.DamagePerSecond);
            acidSpray.AddTransform(sceneReferences.LaserShootTransform);
            acidSpray.isPlayerWeaponDirection = true;
            acidSpray.isPlayer = true;
        }
    }
}