using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ProjectileExplodeSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    private readonly IGroup<GameEntity> _flowFieldGroup;

    public ProjectileExplodeSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _flowFieldGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowField, GameMatcher.GroundEnemyFlowField));
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.ProjectileLanded);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasExplodableProjectile && entity.hasDamage && entity.hasPosition;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.isProjectile = false;

            var positionValue = e.position.Value;
            
            var explosion = _contexts.game.CreateEntity();
            explosion.AddPosition(positionValue);
            explosion.AddExplosion(e.explodableProjectile.ExplosionRadius);
            explosion.AddDamage(e.damage.Value);
            explosion.AddAssetLink(_contexts.game.gameSetup.value.FireballSetings.ExplosionVisualPrefab);
                
            CreateFlowFieldObstacle(e, positionValue);
        }
    }
    private void CreateFlowFieldObstacle(GameEntity e, Vector3 positionValue)
    {
        var flowField = _flowFieldGroup.GetSingleEntity().flowField;
        var flowFieldObstacle = _contexts.game.CreateEntity();
        var flowFieldSettings = _contexts.game.gameSetup.value.FlowFieldSettings;

        var cellsCount = Mathf.RoundToInt(e.explodableProjectile.ExplosionRadius / flowField.CellSize * flowFieldSettings.ExplosionRepulsionSizeMultiplier);

        flowFieldObstacle.AddFlowFieldTemporaryObstacle(cellsCount, flowFieldSettings.ExplosionRepulsionValue);
        flowFieldObstacle.AddPosition(positionValue);
        flowFieldObstacle.AddTimer(flowFieldSettings.ExplosionRepulsionTime);
    }
}