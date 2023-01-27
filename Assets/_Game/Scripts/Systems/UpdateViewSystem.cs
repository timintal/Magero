using System.Collections.Generic;
using Entitas;

public class UpdateViewSystem : ReactiveSystem<GameEntity>
{
    private Contexts _contexts;
    public UpdateViewSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }
    
    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Position.Added(), GameMatcher.Rotation.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasPosition && entity.hasRotation && entity.hasTransform;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.transform.Transform.SetPositionAndRotation(entity.position.Value, entity.rotation.Value);
        }
    }
}