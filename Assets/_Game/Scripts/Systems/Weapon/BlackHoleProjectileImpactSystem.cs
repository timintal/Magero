using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class BlackHoleProjectileImpactSystem: ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    private readonly IGroup<GameEntity> _flowFieldGroup;

    public BlackHoleProjectileImpactSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _flowFieldGroup = contexts.game.GetGroup(GameMatcher.FlowField);
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.ProjectileLanded);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasBlackHoleProjectile && entity.hasDamage && entity.hasPosition;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.isProjectile = false;

            var positionValue = e.position.Value;
            
            var blackHole = _contexts.game.CreateEntity();
            blackHole.AddPosition(positionValue);
            blackHole.AddRotation(Quaternion.identity);
            blackHole.AddRadius(e.blackHoleProjectile.PullRadius);
            blackHole.AddScale(e.blackHoleProjectile.PullRadius * Vector3.one);
            blackHole.AddDamage(e.damage.Value);
            blackHole.AddAttacker(e.attacker.TargetType, e.attacker.TargetMask);
            blackHole.AddResource(e.blackHoleProjectile.BlackHolePrefab);
            blackHole.AddTimer(e.blackHoleProjectile.Lifetime);
            blackHole.AddBlackHole(e.blackHoleProjectile.ExplosionRadius, e.blackHoleProjectile.PullSpeed);
                
            CreateFlowFieldObstacle(e, positionValue);
        }
    }
    private void CreateFlowFieldObstacle(GameEntity e, Vector3 positionValue)
    {
        var flowField = _flowFieldGroup.GetSingleEntity().flowField;
        var flowFieldObstacle = _contexts.game.CreateEntity();
        var flowFieldSettings = _contexts.game.gameSetup.value.FlowFieldSettings;

        var cellsCount = Mathf.RoundToInt(e.blackHoleProjectile.PullRadius / flowField.CellSize * flowFieldSettings.ExplosionRepulsionSizeMultiplier);

        flowFieldObstacle.AddFlowFieldTemporaryObstacle(cellsCount, flowFieldSettings.ExplosionRepulsionValue);
        flowFieldObstacle.AddPosition(positionValue);
        flowFieldObstacle.AddTimer(flowFieldSettings.ExplosionRepulsionTime);
    }
}