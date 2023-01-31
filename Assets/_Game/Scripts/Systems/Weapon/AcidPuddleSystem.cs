using Entitas;
using UnityEngine;

public class AcidPuddleSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _acidPuddleGroup;

    private Collider[] _queryResults;
    
    public AcidPuddleSystem(Contexts contexts)
    {
        _contexts = contexts;
        _acidPuddleGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.AcidPuddle, GameMatcher.Transform, GameMatcher.Radius));
        _queryResults = new Collider[2048];
    }

    public void Execute()
    {
        var colliderCacheMap = _contexts.game.colliderCache.ColliderCacheMap;
        
        foreach (var e in _acidPuddleGroup.GetEntities())
        {
            var newRadius = e.radius.Value - e.acidPuddle.RadiusDecreaseSpeed * Time.deltaTime;

            if (newRadius > 0.1f)
            {
                e.ReplaceRadius(newRadius);

                e.transform.Transform.localScale = Vector3.one * newRadius;
                
                var count = Physics.OverlapSphereNonAlloc(e.transform.Transform.position, newRadius, _queryResults, LayerMask.GetMask("Enemy"));
                
                for (int i = 0; i < count; i++)
                {
                    if (colliderCacheMap.ContainsKey(_queryResults[i]))
                    {
                        var enemyEntity = _contexts.game.GetEntityWithId(colliderCacheMap[_queryResults[i]]);
                        if (enemyEntity.hasTarget && enemyEntity.target.TargetType == TargetType.Enemy)
                        {
                            float totalDamage = e.acidPuddle.DamagePerSecond * Time.deltaTime;
                            if (enemyEntity.hasFloatDamage)
                            {
                                totalDamage += enemyEntity.floatDamage.Value;
                            }
                            
                            enemyEntity.ReplaceFloatDamage(totalDamage);
                        }
                    }
                }
            }
            else
            {
                e.isDestroyed = true;
            }
        }
    }
}