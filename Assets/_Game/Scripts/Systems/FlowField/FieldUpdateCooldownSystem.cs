using System.Collections.Generic;
using Entitas;

public class FieldUpdateCooldownSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public FieldUpdateCooldownSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.TimerCompleted);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasFieldUpdateCooldown && entity.hasEntityRef;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var targetFlowField = _contexts.game.GetEntityWithId(e.entityRef.EntityId);
            targetFlowField.hasFieldUpdateCooldown = false;
        }
    }
}