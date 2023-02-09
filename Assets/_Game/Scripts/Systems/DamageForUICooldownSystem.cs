using Entitas;
using UnityEngine;

public class DamageForUICooldownSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private IGroup<UIEntity> _damageForUIGroup;

    public DamageForUICooldownSystem(Contexts contexts)
    {
        _contexts = contexts;
        _damageForUIGroup = contexts.uI.GetGroup(UIMatcher.AllOf(UIMatcher.DamageForUI, UIMatcher.Position, UIMatcher.MaxHealth));
    }
    public void Execute()
    {
        foreach (var e in _damageForUIGroup)
        {
            float newAccumulatedDamage = e.damageForUI.AccumulatedDamage;
            
            if (e.damageForUI.Value > 0.2f)
            {
               SpawnUIPrefab(e.damageForUI.Value, e);
            }
            else if (e.damageForUI.Value > 0)
            {
                newAccumulatedDamage += e.damageForUI.Value;
            }

            var newCooldown = e.damageForUI.Cooldown - Time.deltaTime;
            var targetEntity = _contexts.game.GetEntityWithId(e.damageForUI.TargetId);
            if (newCooldown <= 0 || targetEntity == null || targetEntity.isDestroyed)
            {
                if (e.damageForUI.AccumulatedDamage > 0.1f)
                {
                    SpawnUIPrefab(newAccumulatedDamage, e);

                    e.ReplaceDamageForUI(
                        e.damageForUI.TargetId,
                        -1,
                        0.5f,
                        0);
                }
                else
                {
                    e.isDestroyed = true;
                }
            }
            else
            {
                e.ReplaceDamageForUI(
                    e.damageForUI.TargetId,
                    -1,
                    newCooldown,
                    newAccumulatedDamage);
            }
        }
    }

    private void SpawnUIPrefab(float damage, UIEntity e)
    {
        var uiEntity = _contexts.game.CreateEntity();
        uiEntity.isUIOverlay = true;
        uiEntity.AddResource(_contexts.game.gameSetup.value.DamageUIOverlayPrefab);
        uiEntity.AddDamage(damage);
        uiEntity.AddPosition(e.position.Value);
        uiEntity.AddMaxHealth(e.maxHealth.Value);
        uiEntity.AddAutoDestruction(1f);
    }
}