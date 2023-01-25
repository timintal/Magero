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
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Damage));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasDamage && entity.hasHealth;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var healthLeft = e.health.Value - e.damage.Damage;
            e.ReplaceHealth(healthLeft);
            if (healthLeft <= 0)
            {
                e.isDestroyed = true;
            }
            e.RemoveDamage();
        }
    }
}