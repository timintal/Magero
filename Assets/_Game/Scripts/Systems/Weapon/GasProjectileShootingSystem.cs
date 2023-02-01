using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class GasProjectileShootingSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;

    public GasProjectileShootingSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(
            GameMatcher.WeaponCooldown.Removed(),
            GameMatcher.WeaponDisabled.Removed(),
            GameMatcher.GasProjectileShooter.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return
            entity.hasProjectileShooter &&
            entity.hasExplodableProjectileShooter &&
            entity.hasTransform &&
            entity.hasDirection && 
            !entity.isWeaponDisabled &&
            !entity.hasWeaponCooldown;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var projectileEntity = _contexts.game.CreateEntity();
            projectileEntity.AddProjectile(e.projectileShooter.Target);
            projectileEntity.AddResource(e.projectileShooter.Prefab);
            projectileEntity.AddPosition(e.transform.Transform.position);
            projectileEntity.AddRotation(Quaternion.identity);
            projectileEntity.AddDirection(e.direction.Value);
            projectileEntity.AddSpeed(e.projectileShooter.ProjectileSpeed, e.projectileShooter.ProjectileSpeed);
            projectileEntity.AddAutoDestruction(7);
            projectileEntity.AddEntityRef(e.id.Value);
            // projectileEntity.AddExplodableProjectile(e.gasProjectileShooter.ExplosionRadius, e.gasProjectileShooter.Damage);

            WeaponCooldownComponent.StartWeaponCooldown(e, e.projectileShooter.Cooldown, _contexts.game);
        }
    }

    
}