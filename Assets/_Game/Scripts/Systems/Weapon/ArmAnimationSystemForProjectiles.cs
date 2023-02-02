using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ArmAnimationSystemForProjectiles : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    private static readonly int Projectile = Animator.StringToHash("projectile");

    public ArmAnimationSystemForProjectiles(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.WeaponCooldown);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isPlayer && 
               entity.hasProjectileShooter 
               && entity.hasWeaponCooldown && 
               entity.hasAnimator;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.animator.Value.SetTrigger(Projectile);
        }
    }
}