using System.Collections.Generic;
using Entitas;

public class ApplyStunSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;

    public ApplyStunSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Stunned.AddedOrRemoved());
    }

    protected override bool Filter(GameEntity entity)
    {
        return true;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if (e.hasAnimator)
            {
                e.animator.Value.enabled = !e.hasStunned;
            }
        }
    }
}