using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ExplosionSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    private IGroup<GameEntity> _targetsGroup;

    public ExplosionSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _targetsGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Target, GameMatcher.Health, GameMatcher.Position));
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Explosion.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasExplosion && 
               entity.hasPosition && 
               entity.hasDamage;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var explosionRadiusSqr = e.explosion.Radius * e.explosion.Radius;
            var damage = e.damage.Damage;
            var position = e.position.Value;

            AddVisualization(position, e);

            foreach (var target in _targetsGroup.GetEntities())
            {
                var diff = target.position.Value - position;
                if (diff.sqrMagnitude < explosionRadiusSqr)
                {
                    int totalDamage = damage;
                    if (target.hasDamage)
                    {
                        totalDamage += target.damage.Damage;
                    }
                    
                    target.ReplaceDamage(totalDamage);
                }
            }
        }
    }

    private void AddVisualization(Vector3 position, GameEntity e)
    {
        var visualizationEntity = _contexts.game.CreateEntity();
        visualizationEntity.AddExplosionVisualization(1);
        visualizationEntity.AddPosition(position + Vector3.up * 0.1f);
        visualizationEntity.AddRadius(e.explosion.Radius * 2);
        visualizationEntity.AddResource(_contexts.game.gameSetup.value.TestWeaponSettings.ExplosionVisualPrefab);
    }
}