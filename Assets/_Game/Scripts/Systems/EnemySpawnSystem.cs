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
                enemyEntity.AddResource(_contexts.game.gameSetup.value.TestEnemySettings.Prefab);
                enemyEntity.AddSpeed(_contexts.game.gameSetup.value.TestEnemySettings.Speed);
                enemyEntity.AddHealth(_contexts.game.gameSetup.value.TestEnemySettings.Health);
                enemyEntity.AddMaxHealth(_contexts.game.gameSetup.value.TestEnemySettings.Health);
                enemyEntity.AddTarget(TargetType.Enemy);
                enemyEntity.isFlowFieldMover = true;
                enemyEntity.isRagdollDeath = true;
                
                var randomPart = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                enemyEntity.AddPosition(e.position.Value + randomPart);
                enemyEntity.AddRotation(Quaternion.identity);
            }

            e.isDestroyed = true;
        }
    }
}