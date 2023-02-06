using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ColliderCacheSystem : ReactiveSystem<GameEntity>, IInitializeSystem, ITearDownSystem
{
    Contexts _contexts;

    public ColliderCacheSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _contexts.game.OnEntityWillBeDestroyed += GameOnEntityWillBeDestroyed;
    }

    private void GameOnEntityWillBeDestroyed(IContext context, IEntity entity)
    {
        if (entity is GameEntity gameEntity)
        {
            if (gameEntity.hasCollider)
            {
                var colliderCacheMap = _contexts.game.colliderCache.ColliderCacheMap;
                if (colliderCacheMap.ContainsKey(gameEntity.collider.Collider))
                {
                    colliderCacheMap.Remove(gameEntity.collider.Collider);
                }
            }
        }
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Collider.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCollider && entity.hasId;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        var colliderCacheMap = _contexts.game.colliderCache.ColliderCacheMap;
        foreach (var e in entities)
        {
            colliderCacheMap.Add(e.collider.Collider, e.id.Value);
        }
    }

    public void Initialize()
    {
        _contexts.game.SetColliderCache(new Dictionary<Collider, int>());
    }

    public void TearDown()
    {
        _contexts.game.OnEntityWillBeDestroyed -= GameOnEntityWillBeDestroyed;
        
    }
}