using Entitas;
using UnityEngine;


public class ProjectileCollisionDetectionSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _projectileGroup;
    private RaycastHit[] _hitsCache;

    public ProjectileCollisionDetectionSystem(Contexts contexts)
    {
        _contexts = contexts;
        _hitsCache = new RaycastHit[100];
        _projectileGroup = _contexts.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Projectile,
            GameMatcher.Position,
            GameMatcher.Direction,
            GameMatcher.Speed,
            GameMatcher.Radius));
    }

    public void Execute()
    {
        foreach (var e in _projectileGroup)
        {
            var distance = e.direction.Value * Time.deltaTime * e.speed.Value;

            int count = Physics.SphereCastNonAlloc(e.position.Value, e.radius.Value, e.direction.Value, _hitsCache,
                distance.magnitude);
            
            for (int i = 0; i < count; i++)
            {
                var colliderCacheMap = _contexts.game.colliderCache.ColliderCacheMap;
                if (colliderCacheMap.ContainsKey(_hitsCache[i].collider))
                {
                    var hitEntity = _contexts.game.GetEntityWithId(colliderCacheMap[_hitsCache[i].collider]);

                    if (hitEntity.hasTarget &&
                        (hitEntity.target.TargetType & e.projectile.Targets) > 0)
                    {
                        if (hitEntity.hasDamage)
                        {
                            hitEntity.ReplaceDamage(hitEntity.damage.Damage + e.projectile.Damage);
                        }
                        else
                        {
                            hitEntity.AddDamage(e.projectile.Damage);
                        }

                        e.isDestroyed = true;
                    }
                }
            }
        }
    }
}