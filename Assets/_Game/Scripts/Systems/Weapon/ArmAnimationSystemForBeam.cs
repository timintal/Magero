using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ArmAnimationSystemForProjectiles : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    private static readonly int Fireball = Animator.StringToHash("fireball");

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
               (entity.hasProjectileShooter || entity.hasLightningShooter) 
               && entity.hasWeaponCooldown && 
               entity.hasAnimator;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.animator.Value.SetTrigger(Fireball);
        }
    }
}

public class ArmAnimationBeamSystem : ReactiveSystem<GameEntity>
{
    private static readonly int Laser = Animator.StringToHash("laser");
    Contexts _contexts;

    public ArmAnimationBeamSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.WeaponDisabled.AddedOrRemoved());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isPlayer && entity.hasAnimator &&
               (entity.isLaserShooter || entity.hasAcidStream || entity.hasWindBlower);
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.animator.Value.SetBool(Laser, !e.isWeaponDisabled);
        }
    }
}