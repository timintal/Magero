using System.Collections.Generic;
using Entitas;
using UnityEngine;


public class GasProjectileImpactSystem: ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    private readonly IGroup<GameEntity> _flowFieldGroup;

    public GasProjectileImpactSystem(Contexts contexts) : base(contexts.game)
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
        return entity.hasGasProjectile && entity.hasDamage && entity.hasPosition;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.isProjectile = false;

            var positionValue = e.position.Value;
            
            var gasCloud = _contexts.game.CreateEntity();
            gasCloud.AddPosition(positionValue);
            gasCloud.AddRotation(Quaternion.identity);
            gasCloud.isDamageOverTimeZone = true;
            gasCloud.AddSpeedModifierZone(e.gasProjectile.MoveSpeedMultiplier);
            gasCloud.AddRadius(e.gasProjectile.CloudRadius);
            gasCloud.AddScale(e.gasProjectile.CloudRadius * Vector3.one);
            gasCloud.AddDamage(e.damage.Value);
            gasCloud.AddAttacker(e.attacker.TargetType, e.attacker.TargetMask);
            gasCloud.AddResource(e.gasProjectile.CloudPrefab);
            gasCloud.AddAutoDestruction(e.gasProjectile.CloudLifetime);
                
            CreateFlowFieldObstacle(e, positionValue);
        }
    }
    private void CreateFlowFieldObstacle(GameEntity e, Vector3 positionValue)
    {
        var flowField = _flowFieldGroup.GetSingleEntity().flowField;
        var flowFieldObstacle = _contexts.game.CreateEntity();
        var flowFieldSettings = _contexts.game.gameSetup.value.FlowFieldSettings;

        var cellsCount = Mathf.RoundToInt(e.gasProjectile.CloudRadius / flowField.CellSize * flowFieldSettings.ExplosionRepulsionSizeMultiplier);

        flowFieldObstacle.AddFlowFieldTemporaryObstacle(cellsCount, flowFieldSettings.ExplosionRepulsionValue);
        flowFieldObstacle.AddPosition(positionValue);
        flowFieldObstacle.AddTimer(flowFieldSettings.ExplosionRepulsionTime);
    }
}