using Entitas;
using UnityEngine;

public class LaserShootingSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _laserShooterGroup;
    private IGroup<GameEntity> _colliderCacheGroup;

    public LaserShootingSystem(Contexts contexts)
    {
        _contexts = contexts;
        _laserShooterGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.LaserShooter, GameMatcher.Transform).NoneOf(GameMatcher.WeaponDisabled));
        _colliderCacheGroup = _contexts.game.GetGroup(GameMatcher.ColliderCache);
    }

    public void Execute()
    {
        var colliderCacheColliderCacheMap = _colliderCacheGroup.GetSingleEntity().colliderCache.ColliderCacheMap;

        foreach (var e in _laserShooterGroup.GetEntities())
        {
            var laserShooter = e.laserShooter;

            if (Physics.Raycast(e.transform.Transform.position,
                    e.direction.Value,
                    out RaycastHit hit,
                    1000,
                    LayerMask.GetMask("Enemy", "Environment")))
            {
                if (colliderCacheColliderCacheMap.ContainsKey(hit.collider))
                {
                    var enemy = _contexts.game.GetEntityWithId(colliderCacheColliderCacheMap[hit.collider]);
                    float totalDamage = Time.deltaTime * laserShooter.DamagePerSecond;
                    if (enemy.hasFloatDamage)
                    {
                        totalDamage += enemy.floatDamage.Value;
                    }
                    enemy.ReplaceFloatDamage(totalDamage);
                    enemy.ReplaceDamageSourcePosition(hit.point - hit.normal);
                }

                e.ReplaceLaserHitPoint(hit.point);
            }
            else
            {
                e.ReplaceLaserHitPoint(e.transform.Transform.position);
            }
            
            
        }
    }
}