using System.Collections.Generic;
using Entitas;

public class LevelFinishSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;

    public LevelFinishSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.LevelFinished);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isLevelFinished;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {

        }
    }
}