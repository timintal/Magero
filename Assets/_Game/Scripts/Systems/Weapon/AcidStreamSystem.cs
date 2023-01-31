using System.Collections.Generic;
using Entitas;
using UnityEngine;


public class AcidStreamSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    private IGroup<GameEntity> _acidPuddleGroup;

    public AcidStreamSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _acidPuddleGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.AcidPuddle, GameMatcher.Position));
        
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(
            GameMatcher.AcidStream.Added(),
            GameMatcher.WeaponCooldown.Removed(),
            GameMatcher.WeaponDisabled.Removed());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasAcidStream &&
               !entity.isWeaponDisabled &&
               !entity.hasWeaponCooldown;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if (Physics.Raycast(e.transform.Transform.position,
                    e.direction.Value,
                    out RaycastHit hit,
                    1000,
                    LayerMask.GetMask("Environment")))
            {
                bool hitExistingPuddle = false;
                foreach (var puddleEntity in _acidPuddleGroup.GetEntities())
                {
                    var sqrRadius = puddleEntity.radius.Value * puddleEntity.radius.Value;
                    if ((hit.point - puddleEntity.position.Value).sqrMagnitude < sqrRadius)
                    {
                        hitExistingPuddle = true;
                        puddleEntity.ReplaceRadius(e.acidStream.PoolRadius);
                        break;
                    }
                }

                if (!hitExistingPuddle)
                {
                    var newPuddleEntity = _contexts.game.CreateEntity();
                    newPuddleEntity.AddAcidPuddle(e.acidStream.DamagePerSecond, e.acidStream.RadiusDecreasePerSecond);
                    newPuddleEntity.AddRadius(e.acidStream.PoolRadius);
                    newPuddleEntity.AddPosition(hit.point);
                    newPuddleEntity.AddRotation(Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
                    newPuddleEntity.AddResource(e.acidStream.PoolPrefab);
                }
            }
            
            WeaponCooldownComponent.StartWeaponCooldown(e, e.acidStream.Cooldown, _contexts.game);
        }
    }
}