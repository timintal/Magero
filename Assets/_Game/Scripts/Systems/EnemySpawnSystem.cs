using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class EnemySpawnSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    private IGroup<GameEntity> _groundEnemyFlowFieldGroup;
    private IGroup<GameEntity> _flyingEnemyFlowFieldGroup;

    public EnemySpawnSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _groundEnemyFlowFieldGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowField, GameMatcher.GroundEnemyFlowField));
        _flyingEnemyFlowFieldGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowField, GameMatcher.FlyingEnemyFlowField));
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
        if (entities.Count == 0) return;
        
        var groundFlowFieldId = _groundEnemyFlowFieldGroup.GetSingleEntity().id.Value;
        var flyingFlowFieldId = _flyingEnemyFlowFieldGroup.GetSingleEntity().id.Value;
        
        foreach (var e in entities)
        {
            for (int i = 0; i < e.enemySpawnRequest.Count; i++)
            {
                var enemyEntity = _contexts.game.CreateEntity();
                enemyEntity.AddResource(e.enemySpawnRequest.EnemySettings.Prefab);
                enemyEntity.AddSpeed(e.enemySpawnRequest.EnemySettings.Speed, e.enemySpawnRequest.EnemySettings.Speed);
                enemyEntity.AddHealth(e.enemySpawnRequest.EnemySettings.Health);
                enemyEntity.AddMaxHealth(e.enemySpawnRequest.EnemySettings.Health);
                enemyEntity.AddRadius(e.enemySpawnRequest.EnemySettings.Radius);
                enemyEntity.AddTarget(TargetType.Enemy);
                enemyEntity.AddAnimatorSpeedSync(Animator.StringToHash("SpeedFactor"));
                enemyEntity.AddFlowFieldMover(e.enemySpawnRequest.EnemySettings.IsFlying ? flyingFlowFieldId : groundFlowFieldId);
                enemyEntity.isRagdollDeath = true;

                var randPart = new Vector3(Random.Range(-e.enemySpawnRequest.Bounds.x * 0.5f, e.enemySpawnRequest.Bounds.x * 0.5f), 0,
                    Random.Range(e.enemySpawnRequest.Bounds.y * 0.5f, e.enemySpawnRequest.Bounds.y * 0.5f));

                enemyEntity.AddPosition(e.position.Value  + randPart);
                enemyEntity.AddRotation(Quaternion.identity);
            }

            e.isDestroyed = true;
        }
    }
}