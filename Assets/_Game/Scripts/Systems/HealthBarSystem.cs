using System.Collections.Generic;
using Entitas;

public class HealthBarSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;

    public HealthBarSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Health);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasHealth && entity.hasMaxHealth && entity.hasHealthBarUI;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.healthBarUI.FillBar.fillAmount = (float)e.health.Value / e.maxHealth.Value;
        }
    }
}