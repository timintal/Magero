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
        _laserShooterGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.LaserShooter, GameMatcher.Transform, GameMatcher.Damage).NoneOf(GameMatcher.WeaponDisabled));
        _colliderCacheGroup = _contexts.game.GetGroup(GameMatcher.ColliderCache);
    }

    public void Execute()
    {
        var colliderCacheColliderCacheMap = _colliderCacheGroup.GetSingleEntity().colliderCache.ColliderCacheMap;

        foreach (var e in _laserShooterGroup.GetEntities())
        {
            if (Physics.Raycast(e.transform.Transform.position,
                    e.direction.Value,
                    out RaycastHit hit,
                    1000,
                    e.attacker.TargetMask))
            {
                if (colliderCacheColliderCacheMap.ContainsKey(hit.collider))
                {
                    var enemy = _contexts.game.GetEntityWithId(colliderCacheColliderCacheMap[hit.collider]);
                    
                    float totalDamage = Time.deltaTime * e.damage.Value;

                    var damageEntity = _contexts.game.CreateEntity();
                    damageEntity.AddReceivedDamage(totalDamage);
                    damageEntity.AddEntityRef(enemy.id.Value);

                    enemy.ReplaceDamageSourcePosition(hit.point - hit.normal);
                }

                e.ReplaceWeaponHitPoint(hit.point);
            }
            else
            {
                e.ReplaceWeaponHitPoint(new Vector3(-100,-100,-100));
            }
            
            
        }
    }
}