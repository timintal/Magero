using Entitas;
using UnityEngine;

public class DamageOverTimeZoneSystem : IExecuteSystem
{
   Contexts _contexts;
    private IGroup<GameEntity> _damageZonesGroup;

    private Collider[] _queryResults;
    
    public DamageOverTimeZoneSystem(Contexts contexts)
    {
        _contexts = contexts;
        _damageZonesGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.DamageOverTimeZone, GameMatcher.Position, GameMatcher.Radius));
        _queryResults = new Collider[2048];
    }

    public void Execute()
    {
        var colliderCacheMap = _contexts.game.colliderCache.ColliderCacheMap;
        
        foreach (var e in _damageZonesGroup.GetEntities())
        {
            var count = Physics.OverlapSphereNonAlloc(e.position.Value, e.radius.Value, _queryResults, LayerMask.GetMask("Enemy"));
                
            for (int i = 0; i < count; i++)
            {
                if (colliderCacheMap.ContainsKey(_queryResults[i]))
                {
                    var enemyEntity = _contexts.game.GetEntityWithId(colliderCacheMap[_queryResults[i]]);
                    if (enemyEntity.hasTarget && enemyEntity.target.TargetType == TargetType.Enemy)
                    {
                        float totalDamage = e.damage.Value * Time.deltaTime;

                        var damageEntity = _contexts.game.CreateEntity();
                        damageEntity.AddReceivedDamage(totalDamage);
                        damageEntity.AddEntityRef(enemyEntity.id.Value);
                        
                        enemyEntity.ReplaceDamageSourcePosition(e.position.Value);
                    }
                }
            }
        }
    }
}