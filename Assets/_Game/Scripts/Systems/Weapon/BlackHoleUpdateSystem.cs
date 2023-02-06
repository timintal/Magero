using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class BlackHoleUpdateSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _blackHoleGroup;
    private Collider[] _queryResults;

    public BlackHoleUpdateSystem(Contexts contexts)
    {
        _contexts = contexts;
        _blackHoleGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.BlackHole, 
            GameMatcher.Position, 
            GameMatcher.Radius,
            GameMatcher.Attacker,
            GameMatcher.AssetLink));
        _queryResults = new Collider[2048];
    }

    public void Execute()
    {
        var colliderCacheMap = _contexts.game.colliderCache.ColliderCacheMap;

        foreach (var e in _blackHoleGroup.GetEntities())
        {
            var count = Physics.OverlapSphereNonAlloc(e.position.Value, e.radius.Value, _queryResults, e.attacker.TargetMask);

            for (int i = 0; i < count; i++)
            {
                Collider c = _queryResults[i];

                if (colliderCacheMap.ContainsKey(c))
                {
                    var enemy = _contexts.game.GetEntityWithId(colliderCacheMap[c]);
                    if (enemy.hasTarget && enemy.hasPosition && (enemy.target.TargetType & e.attacker.TargetType) > 0)
                    {
                        var diff = e.position.Value - enemy.position.Value;

                        var distance = diff.magnitude;

                        if (distance > 1)
                        {
                            var forceMovementEntity = _contexts.game.CreateEntity();
                            forceMovementEntity.AddForcedMovement(e.blackHole.PullSpeed);
                            forceMovementEntity.AddDirection(diff/distance);
                            forceMovementEntity.AddEntityRef(enemy.id.Value);
                        }
                    }
                }
            }

            if (e.isTimerCompleted)
            {
                var explosionEntity = _contexts.game.CreateEntity();
                explosionEntity.AddPosition(e.position.Value);
                explosionEntity.AddExplosion(e.blackHole.ExplosionRadius);
                explosionEntity.AddDamage(e.damage.Value);
                explosionEntity.AddAssetLink(e.assetLink.Asset);
            }
        }
    }
}