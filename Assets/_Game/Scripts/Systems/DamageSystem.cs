using System.Collections.Generic;
using Entitas;

public class DamageSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public DamageSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.ReceivedDamage));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasReceivedDamage && entity.hasEntityRef;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var targetEntity = _contexts.game.GetEntityWithId(e.entityRef.EntityId);
            if (targetEntity != null && targetEntity.hasHealth)
            {
                var healthLeft = targetEntity.health.Value - e.receivedDamage.Value;
                if (healthLeft < 0)
                {
                    targetEntity.isDestroyed = true;
                }
                else
                {
                    targetEntity.ReplaceHealth(healthLeft);
                }
            }

            e.isDestroyed = true;
        }
    }
}