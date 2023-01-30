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

        if (gameSetup.AddFireballs)
        {
            var weaponEntity = _contexts.game.CreateEntity();

            weaponEntity.AddProjectileShooter(gameSetup.FireballSetings.Cooldown,
                gameSetup.FireballSetings.ProjectilePrefab,
                gameSetup.FireballSetings.ProjectileSpeed,
                TargetType.Enemy,
                gameSetup.FireballSetings.Damage);

            weaponEntity.AddExplodableProjectileShooter(gameSetup.FireballSetings.ExplosionRadius);
            weaponEntity.AddHealth(10);
            weaponEntity.AddTransform(sceneReferences.FireballsShootTransform);
            weaponEntity.isPlayerWeaponDirection = true;
            weaponEntity.isPlayer = true;
        }

        if (gameSetup.AddLaser)
        {
            var laserEntity = _contexts.game.CreateEntity();
            laserEntity.AddLaserShooter(sceneReferences.LaserRenderer, gameSetup.LaserSettings.DamagePerSecond);
            laserEntity.AddTransform(sceneReferences.LaserShootTransform);
            laserEntity.AddLaserSparkles(sceneReferences.LaserSparkles);
            laserEntity.AddLaserHitPoint(sceneReferences.LaserShootTransform.position);
            laserEntity.isPlayerWeaponDirection = true;
            laserEntity.isPlayer = true;
        }

    }
}