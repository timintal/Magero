using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class LightningStrikeShootingSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    private IGroup<GameEntity> _colliderCacheGroup;
    private int _layerMask;

    private Collider[] _queryResults;
    public LightningStrikeShootingSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _colliderCacheGroup = _contexts.game.GetGroup(GameMatcher.ColliderCache);
        _layerMask = LayerMask.GetMask("Enemy", "Environment");
        _queryResults = new Collider[4096];
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.LightningShooter.Added(),
            GameMatcher.WeaponCooldown.Removed(),
            GameMatcher.WeaponDisabled.Removed());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasLightningShooter && 
               entity.hasTransform && 
               entity.hasDirection && 
               entity.hasAssetLink &&
               !entity.isWeaponDisabled &&
               !entity.hasWeaponCooldown;
    }

    protected override void Execute(List<GameEntity> entities)
    {
         var colliderCacheColliderCacheMap = _colliderCacheGroup.GetSingleEntity().colliderCache.ColliderCacheMap;

        foreach (var e in entities)
        {
            if (Physics.Raycast(e.transform.Transform.position,
                    e.direction.Value,
                    out RaycastHit hit,
                    1000,
                    _layerMask))
            {
                if (colliderCacheColliderCacheMap.ContainsKey(hit.collider))
                {
                    var enemy = _contexts.game.GetEntityWithId(colliderCacheColliderCacheMap[hit.collider]);
                    if (enemy.hasTarget && enemy.target.TargetType == TargetType.Enemy)
                    {
                        int totalDamage = e.lightningShooter.TargetDamage;
                        var damageEntity = _contexts.game.CreateEntity();
                        damageEntity.AddReceivedDamage(totalDamage);
                        damageEntity.AddEntityRef(enemy.id.Value);
                        
                        enemy.ReplaceDamageSourcePosition(hit.point - hit.normal);
                    }
                }

                var collidersCount = Physics.OverlapSphereNonAlloc(hit.point, e.lightningShooter.EffectRadius, _queryResults, LayerMask.GetMask("Enemy"));
                for (int i = 0; i < collidersCount; i++)
                {
                    if (colliderCacheColliderCacheMap.ContainsKey(_queryResults[i]))
                    {
                        var enemy = _contexts.game.GetEntityWithId(colliderCacheColliderCacheMap[_queryResults[i]]);
                        if (enemy.hasTarget && enemy.target.TargetType == TargetType.Enemy)
                        {
                            int totalDamage = e.lightningShooter.AOEDamage;
                            
                            var damageEntity = _contexts.game.CreateEntity();
                            damageEntity.AddReceivedDamage(totalDamage);
                            damageEntity.AddEntityRef(enemy.id.Value);
                            
                            enemy.ReplaceDamageSourcePosition(hit.point - hit.normal);
                            enemy.ReplaceStunned(e.lightningShooter.StunDuration);
                        }
                    }
                }
                AddVisualization(hit.point, e);
            }
            
            WeaponCooldownComponent.StartWeaponCooldown(e, e.lightningShooter.Cooldown, _contexts.game);
           
        }
    }
    
    private void AddVisualization(Vector3 position, GameEntity e)
    {
        var visualizationEntity = _contexts.game.CreateEntity();
        visualizationEntity.AddExplosionVisualization(5);
        visualizationEntity.AddPosition(position + Vector3.up * 0.1f);
        visualizationEntity.AddRotation(Quaternion.identity);
        visualizationEntity.AddRadius(e.lightningShooter.EffectRadius);
        visualizationEntity.AddResource(e.assetLink.Asset);
    }
    
}