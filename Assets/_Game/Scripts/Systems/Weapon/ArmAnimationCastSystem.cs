using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ArmAnimationCastSystem : ReactiveSystem<GameEntity>
{
    private static readonly int Cast = Animator.StringToHash("cast");
    Contexts _contexts;

    public ArmAnimationCastSystem(Contexts contexts) : base(contexts.game)
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
               (entity.hasSummonSpell || entity.hasLightningShooter);
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.animator.Value.SetBool(Cast, !e.isWeaponDisabled);
        }
    }
}