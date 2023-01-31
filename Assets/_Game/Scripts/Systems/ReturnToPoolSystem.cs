using System.Collections.Generic;
using Entitas;
using Entitas.Unity;

public class ReturnToPoolSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    private readonly IGroup<GameEntity> _colliderCacheGroup;

    public ReturnToPoolSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _colliderCacheGroup = _contexts.game.GetGroup(GameMatcher.ColliderCache);
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Destroyed);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasTransform && entity.isDestroyed;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        var colliderCache = _colliderCacheGroup.GetSingleEntity().colliderCache.ColliderCacheMap;

        foreach (var e in entities)
        {
            var entityLink = e.transform.Transform.gameObject.GetEntityLink();
            if (entityLink != null)
            {
                entityLink.Unlink();
            }
            
            var poolableMonoBehaviour = e.transform.Transform.gameObject.GetComponent<PoolableMonoBehaviour>();
            
            if (poolableMonoBehaviour != null)
            {
                poolableMonoBehaviour.ParentPool.Release(poolableMonoBehaviour);
            }

            if (e.hasCollider)
            {
                colliderCache.Remove(e.collider.Collider);
            }
        }
    }
}