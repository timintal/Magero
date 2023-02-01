using Entitas;
using UnityEngine;

public class LandingExplosionSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _projectilesGroup;
    private IGroup<GameEntity> _flowFieldGroup;

    public LandingExplosionSystem(Contexts contexts)
    {
        _contexts = contexts;
        _projectilesGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Projectile, GameMatcher.ExplodableProjectile, GameMatcher.Position, GameMatcher.Damage));
        _flowFieldGroup = contexts.game.GetGroup(GameMatcher.FlowField);
    }

    public void Execute()
    {
        foreach (var e in _projectilesGroup.GetEntities())
        {
            var positionValue = e.position.Value;
            if (positionValue.y < 0)
            {
                e.RemoveProjectile();
                
                var explosion = _contexts.game.CreateEntity();
                positionValue.y = 0;
                explosion.AddPosition(positionValue);
                explosion.AddExplosion(e.explodableProjectile.ExplosionRadius);
                explosion.AddDamage(e.damage.Value);
                
                CreateFlowFieldObstacle(e, positionValue);
            }
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