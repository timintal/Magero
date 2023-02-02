using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class SpawnOnDestroyVisualSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;

    public SpawnOnDestroyVisualSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Destroyed);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isDestroyed && entity.hasAssetLink && entity.isOnDestroyFx && entity.hasPosition;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var onDestroyFxEntity = _contexts.game.CreateEntity();
            onDestroyFxEntity.AddResource(e.assetLink.Asset);
            onDestroyFxEntity.AddPosition(e.position.Value);
            onDestroyFxEntity.AddRotation(Quaternion.identity);
            onDestroyFxEntity.AddAutoDestruction(5f);
        }
    }
}