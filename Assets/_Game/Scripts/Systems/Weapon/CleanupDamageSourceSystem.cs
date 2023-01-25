using System.Collections.Generic;
using Entitas;

public class CleanupDamageSourceSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;

    public CleanupDamageSourceSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.DamageSourcePosition);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasDamageSourcePosition;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.RemoveDamageSourcePosition();
        }
    }
}