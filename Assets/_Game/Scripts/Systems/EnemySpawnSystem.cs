using System.Collections.Generic;
using _Game.Data;
using Entitas;
using UnityEngine;

public class EnemySpawnSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    private readonly IUnitHealthProvider _healthProvider;
    private readonly IUnitDamageProvider _unitDamageProvider;
    private readonly PlayerData _playerData;
    private IGroup<GameEntity> _groundEnemyFlowFieldGroup;
    private IGroup<GameEntity> _flyingEnemyFlowFieldGroup;

    public EnemySpawnSystem(Contexts contexts, 
        IUnitHealthProvider healthProvider,
        IUnitDamageProvider unitDamageProvider,
        PlayerData playerData) : base(contexts.game)
    {
        _contexts = contexts;
        _healthProvider = healthProvider;
        _unitDamageProvider = unitDamageProvider;
        _playerData = playerData;
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
                var unitType = e.enemySpawnRequest.EnemySettings.Type;
                var speed = e.enemySpawnRequest.EnemySettings.Speed;
                enemyEntity.AddSpeed(speed, speed);
                var health = _healthProvider.GetHealth(unitType, _playerData.PlayerLevel);
                enemyEntity.AddHealth(health);
                enemyEntity.AddMaxHealth(health);
                enemyEntity.AddDamage(_unitDamageProvider.GetDamage(unitType, _playerData.PlayerLevel));
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