using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public class CreateViewSystem : ReactiveSystem<GameEntity>
{
    private Contexts _contexts;
    public CreateViewSystem(Contexts context) : base(context.game)
    {
        _contexts = context;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Resource.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasResource;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            GameObject obj = Object.Instantiate(entity.resource.Prefab);
         
            entity.AddTransform(obj.transform);
            var collider = obj.GetComponentInChildren<Collider>();
            if (collider != null)
            {
                entity.AddCollider(collider);
            }
            obj.transform.position = entity.position.Value;
            obj.Link(entity);
        }
    }
}