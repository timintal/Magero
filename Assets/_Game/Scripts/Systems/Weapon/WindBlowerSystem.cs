using Entitas;
using UnityEngine;

public class WindBlowerSystem : IExecuteSystem
{
    Contexts _contexts;

    private RaycastHit[] _queryResults;
    private IGroup<GameEntity> _windBlowerGroup;

    public WindBlowerSystem(Contexts contexts) 
    {
        _contexts = contexts;
        _queryResults = new RaycastHit[2048];
        _windBlowerGroup = contexts.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.WindBlower, 
            GameMatcher.Transform, 
            GameMatcher.Radius,
            GameMatcher.Damage,
            GameMatcher.Direction)
            .NoneOf(GameMatcher.WeaponDisabled));
    }


    public void Execute()
    {
        var colliderCacheMap = _contexts.game.colliderCache.ColliderCacheMap;
        foreach (var e in _windBlowerGroup.GetEntities())
        {
            var position = e.transform.Transform.position;
            var count = Physics.SphereCastNonAlloc(position, e.radius.Value, e.direction.Value, _queryResults, 100);
            for (int i = 0; i < count; i++)
            {
                if (colliderCacheMap.ContainsKey(_queryResults[i].collider))
                {
                    var enemy = _contexts.game.GetEntityWithId(colliderCacheMap[_queryResults[i].collider]);
                    if (enemy.hasTarget && enemy.target.TargetType == TargetType.Enemy)
                    {
                        var diff = _queryResults[i].point - position;
                        diff.y = 0;
                        enemy.ReplaceWindImpulse(diff.normalized, e.windBlower.PushSpeed);
                        enemy.ReplaceDamping(e.windBlower.PushDamping);

                        var damageEntity = _contexts.game.CreateEntity();
                        damageEntity.AddReceivedDamage(e.damage.Value * Time.deltaTime);
                        damageEntity.AddEntityRef(enemy.id.Value);
                        
                        enemy.ReplaceDamageSourcePosition(_queryResults[i].point - _queryResults[i].normal);
                    }
                }
            }
        }
    }
}