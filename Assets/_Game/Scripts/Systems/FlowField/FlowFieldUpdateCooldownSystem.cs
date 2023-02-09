using System.Collections.Generic;
using Entitas;

public class FlowFieldUpdateCooldownSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public FlowFieldUpdateCooldownSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.TimerCompleted.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasFieldUpdateCooldown && entity.hasEntityRef;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var flowField = _contexts.game.GetEntityWithId(e.entityRef.EntityId);
            if (flowField != null)
            {
                flowField.hasFieldUpdateCooldown = false;
            }
        }
    }
}