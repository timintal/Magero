using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class BlackHoleShootingSystem: ReactiveSystem<GameEntity>
{
    Contexts _contexts;

    public BlackHoleShootingSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(
            GameMatcher.WeaponCooldown.Removed(),
            GameMatcher.WeaponDisabled.Removed(),
            GameMatcher.BlackHoleShooter.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return
            entity.hasProjectileShooter &&
            entity.hasBlackHoleShooter &&
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
            projectileEntity.isProjectile = true;
            projectileEntity.AddAttacker(e.attacker.TargetType, e.attacker.TargetMask);
            projectileEntity.AddResource(e.projectileShooter.Prefab);
            projectileEntity.AddPosition(e.transform.Transform.position);
            projectileEntity.AddRotation(Quaternion.identity);
            projectileEntity.AddDirection(e.direction.Value);
            projectileEntity.AddSpeed(e.projectileShooter.ProjectileSpeed, e.projectileShooter.ProjectileSpeed);
            projectileEntity.AddAutoDestruction(7);
            projectileEntity.AddAssetLink(e.blackHoleShooter.ExplosionPrefab);
            
            projectileEntity.AddBlackHoleProjectile(
                e.blackHoleShooter.ExplosionRadius, 
                e.blackHoleShooter.BlackHolePullSpeed, 
                e.blackHoleShooter.BlackHolePullRadius, 
                e.blackHoleShooter.BlackHolePrefab,
                e.blackHoleShooter.BlackHoleLifetime);
            
            projectileEntity.AddDamage(e.damage.Value);
            projectileEntity.AddTarget(TargetType.Player);

            WeaponCooldownComponent.StartWeaponCooldown(e, e.projectileShooter.Cooldown, _contexts.game);
        }
    }
}