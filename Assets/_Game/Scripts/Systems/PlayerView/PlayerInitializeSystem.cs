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

        var weaponEntity = _contexts.game.CreateEntity();
        weaponEntity.AddDamage(gameSetup.TestWeaponSettings.Damage);
        weaponEntity.AddProjectileShooter(gameSetup.TestWeaponSettings.Cooldown, gameSetup.TestWeaponSettings.ProjectilePrefab, gameSetup.TestWeaponSettings.ProjectileSpeed);
        weaponEntity.AddExplodableProjectileShooter(gameSetup.TestWeaponSettings.ExplosionRadius);
        weaponEntity.AddTarget(TargetType.Enemy);
        weaponEntity.AddTransform(sceneReferences.ShootTransform);
        weaponEntity.isPlayer = true;
    }
}