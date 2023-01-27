using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class EnemySpawnSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;

    public EnemySpawnSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.EnemySpawnRequest.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasEnemySpawnRequest && entity.hasPosition && !entity.isDestroyed;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            for (int i = 0; i < e.enemySpawnRequest.Count; i++)
            {
                var enemyEntity = _contexts.game.CreateEntity();
                enemyEntity.AddResource(e.enemySpawnRequest.EnemySettings.Prefab);
                enemyEntity.AddSpeed(e.enemySpawnRequest.EnemySettings.Speed);
                enemyEntity.AddHealth(e.enemySpawnRequest.EnemySettings.Health);
                enemyEntity.AddMaxHealth(e.enemySpawnRequest.EnemySettings.Health);
                enemyEntity.AddTarget(TargetType.Enemy);
                enemyEntity.isFlowFieldMover = true;
                enemyEntity.isRagdollDeath = true;
                
                enemyEntity.AddPosition(e.position.Value);
                enemyEntity.AddRotation(Quaternion.identity);
            }

            e.isDestroyed = true;
        }
    }
}