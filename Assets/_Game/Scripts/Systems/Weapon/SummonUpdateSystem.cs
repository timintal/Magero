using Entitas;
using UnityEngine;

public class SummonUpdateSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _summonsGroup;
    private IGroup<GameEntity> _summonFlowField;

    private Collider[] _queryResults;

    public SummonUpdateSystem(Contexts contexts)
    {
        _contexts = contexts;
        _summonsGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Summon, GameMatcher.Position));
        _summonFlowField = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowField, GameMatcher.SummonFlowField));
        _queryResults = new Collider[1024];
    }

    public void Execute()
    {
        var flowField = _summonFlowField.GetSingleEntity().flowField;
        var colliderCacheMap = _contexts.game.colliderCache.ColliderCacheMap;

        foreach (var e in _summonsGroup.GetEntities())
        {
            if (!e.hasDirection)
            {
                Vector3 dir = Vector3.zero;
                float minDist = float.MaxValue;
                bool directionFound = false;
                
                var count = Physics.OverlapSphereNonAlloc(e.position.Value, flowField.CellSize * 2, _queryResults, LayerMask.GetMask("Enemy"));
                for (int i = 0; i < count; i++)
                {
                    if (colliderCacheMap.ContainsKey(_queryResults[i]))
                    {
                        var enemy = _contexts.game.GetEntityWithId(colliderCacheMap[_queryResults[i]]);
                        if (enemy.hasTarget && (enemy.target.TargetType & e.attacker.TargetType) > 0)
                        {
                            Vector3 diff = enemy.position.Value - e.position.Value;

                            var distance = diff.magnitude;
                            if (distance < minDist)
                            {
                                dir = diff / distance;
                                minDist = distance;
                                directionFound = true;
                            }

                            if (distance < e.radius.Value + enemy.radius.Value)
                            {
                                e.isDestroyed = true;
                                
                                var damageEntity = _contexts.game.CreateEntity();
                                damageEntity.AddReceivedDamage(e.damage.Value);
                                damageEntity.AddEntityRef(enemy.id.Value);
                                
                                enemy.ReplaceDamageSourcePosition(e.position.Value);
                                break;
                            }
                        }
                    }
                }

                if (!e.isDestroyed && directionFound)
                {
                    e.ReplacePosition(e.position.Value + e.speed.Value * Time.deltaTime * dir);
                }
                
            }
        }
    }
}