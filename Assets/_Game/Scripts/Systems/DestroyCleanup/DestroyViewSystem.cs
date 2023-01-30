using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using Entitas.VisualDebugging.Unity;
using UnityEngine;

public class DestroyViewSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;

    public DestroyViewSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Destroyed);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isDestroyed && entity.hasTransform;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var entityLink = e.transform.Transform.gameObject.GetEntityLink();
            if (entityLink != null)
            {
                entityLink.Unlink();
            }
            e.transform.Transform.gameObject.DestroyGameObject();
        }
    }
}