using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ExplosionSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    private readonly PoolService _poolService;
    private IGroup<GameEntity> _targetsGroup;

    public ExplosionSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        
        _targetsGroup = _contexts.game.GetGroup(
            GameMatcher.AllOf(
                GameMatcher.Target,
                GameMatcher.Health, 
                GameMatcher.Position, 
                GameMatcher.Radius));
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Explosion.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasExplosion && 
               entity.hasPosition &&
               entity.hasDamage &&
               entity.hasAssetLink;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var explosionRadiusSqr = e.explosion.Radius * e.explosion.Radius;
            var damage = e.damage.Value;
            var position = e.position.Value;

            AddVisualization(position, e);

            foreach (var target in _targetsGroup.GetEntities())
            {
                var diff = target.position.Value - position;
                if (diff.sqrMagnitude < explosionRadiusSqr)
                {
                    ApplyDamage(damage, target);

                    target.ReplaceDamageSourcePosition(position);
                }
            }

            e.isDestroyed = true;   
        }
    }

    private void ApplyDamage(float damage, GameEntity target)
    {
        var damageEntity = _contexts.game.CreateEntity();
        damageEntity.AddReceivedDamage(damage);
        damageEntity.AddEntityRef(target.id.Value);
    }

    private void AddVisualization(Vector3 position, GameEntity e)
    {
        var visualizationEntity = _contexts.game.CreateEntity();
        visualizationEntity.AddExplosionVisualization(5);
        visualizationEntity.AddPosition(position + Vector3.up * 0.1f);
        visualizationEntity.AddRotation(Quaternion.identity);
        visualizationEntity.AddRadius(e.explosion.Radius * 2);
        visualizationEntity.AddResource(e.assetLink.Asset);
    }
}