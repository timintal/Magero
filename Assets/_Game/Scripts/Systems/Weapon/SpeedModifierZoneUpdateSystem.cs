using Entitas;
using UnityEngine;

public class SpeedModifierZoneUpdateSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _speedModifierZoneGroup;

    private Collider[] _queryResults;
    
    public SpeedModifierZoneUpdateSystem(Contexts contexts)
    {
        _contexts = contexts;
        _speedModifierZoneGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.SpeedModifierZone, GameMatcher.Position, GameMatcher.Radius, GameMatcher.Attacker));
        _queryResults = new Collider[2048];
    }

    public void Execute()
    {
        var colliderCacheMap = _contexts.game.colliderCache.ColliderCacheMap;
        
        foreach (var e in _speedModifierZoneGroup.GetEntities())
        {
            var count = Physics.OverlapSphereNonAlloc(e.position.Value, e.radius.Value, _queryResults, e.attacker.TargetMask);
                
            for (int i = 0; i < count; i++)
            {
                if (colliderCacheMap.ContainsKey(_queryResults[i]))
                {
                    var enemyEntity = _contexts.game.GetEntityWithId(colliderCacheMap[_queryResults[i]]);
                    if (enemyEntity.hasTarget && (enemyEntity.target.TargetType & e.attacker.TargetType) > 0)
                    {
                        enemyEntity.ReplaceSpeed(enemyEntity.speed.Value * e.speedModifierZone.Multiplier, enemyEntity.speed.BaseValue);
                    }
                }
            }
        }
    }
}