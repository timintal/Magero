using Entitas;
using UnityEngine;

public class CollectDamageToShowInUI : IExecuteSystem
{
    private readonly Contexts _contexts;
    private IGroup<GameEntity> _damagedGroup;

    public CollectDamageToShowInUI(Contexts contexts)
    {
        _contexts = contexts;
        _damagedGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.ReceivedDamage, GameMatcher.EntityRef));
    }
    
    public void Execute()
    {
        foreach (var e in _damagedGroup.GetEntities())
        {
            var damagedEntity = _contexts.game.GetEntityWithId(e.entityRef.EntityId);
            if (damagedEntity.hasTarget && damagedEntity.target.TargetType == TargetType.Enemy)
            {
                if (damagedEntity.hasPosition && damagedEntity.hasMaxHealth)
                {
                    var entityWithDamageForUI = _contexts.uI.GetEntityWithDamageForUI(e.entityRef.EntityId);
                    
                    if (entityWithDamageForUI != null)
                    {
                        var newAccumulatedDamage = entityWithDamageForUI.damageForUI.AccumulatedDamage + e.receivedDamage.Value;
                        entityWithDamageForUI.ReplaceDamageForUI(
                            e.entityRef.EntityId,
                            entityWithDamageForUI.damageForUI.Value,
                            entityWithDamageForUI.damageForUI.Cooldown,
                            newAccumulatedDamage);
                        entityWithDamageForUI.ReplacePosition(damagedEntity.position.Value);
                    }
                    else
                    {
                        var newUIDamage = _contexts.uI.CreateEntity();
                        newUIDamage.AddDamageForUI(e.entityRef.EntityId, e.receivedDamage.Value, 0.5f, 0);
                        newUIDamage.AddPosition(damagedEntity.position.Value);
                        newUIDamage.AddMaxHealth(damagedEntity.maxHealth.Value);
                    }
                }
            }
        }
    }
}