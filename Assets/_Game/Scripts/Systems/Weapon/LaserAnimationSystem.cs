using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class FireballAnimationSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    private static readonly int Fireball = Animator.StringToHash("fireball");

    public FireballAnimationSystem(Contexts contexts) : base(contexts.game)
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

public class LaserAnimationSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _beamWeaponGroup;
    private static readonly int Laser = Animator.StringToHash("laser");

    public LaserAnimationSystem(Contexts contexts)
    {
        _contexts = contexts;
        _beamWeaponGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.Animator)
            .AnyOf(GameMatcher.LaserShooter, GameMatcher.AcidStream, GameMatcher.WindBlower));
    }

    public void Execute()
    {
        foreach (var e in _beamWeaponGroup.GetEntities())
        {
            e.animator.Value.SetBool(Laser, !e.isWeaponDisabled);
        }
    }
}