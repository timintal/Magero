using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public class CreateViewSystem : ReactiveSystem<GameEntity>, ITearDownSystem
{
    private Contexts _contexts;
    private readonly PoolService _poolService;

    public CreateViewSystem(Contexts context, PoolService poolService) : base(context.game)
    {
        _contexts = context;
        _poolService = poolService;
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
            
            var obj = _poolService.GetGameObject(entity.resource.Prefab);

            entity.AddTransform(obj.transform);
            var collider = obj.GetComponentInChildren<Collider>();
            if (collider != null)
            {
                entity.AddCollider(collider);
            }
            var renderer = obj.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                entity.AddRenderer(renderer);
            }
            
            var animator = obj.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                entity.AddAnimator(animator);
            }

            if (entity.hasScale)
            {
                obj.transform.localScale = entity.scale.Value;
            }
            
            obj.gameObject.Link(entity);
        }
    }

    public void TearDown()
    {
        var viewGroup = _contexts.game.GetGroup(GameMatcher.Transform);
        foreach (var e in viewGroup.GetEntities())
        {
            if (e.transform.Transform == null) continue;
            
            var entityLink = e.transform.Transform.gameObject.GetEntityLink();
            if (entityLink != null)
            {
                entityLink.Unlink();
            }
        }
    }
}