using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class HealthBarSystem : IExecuteSystem
{
    private IGroup<GameEntity> _healthBarEntities;

    public HealthBarSystem(Contexts contexts)
    {
        _healthBarEntities = contexts.game.GetGroup(Matcher<GameEntity>.AllOf(GameMatcher.Health, GameMatcher.MaxHealth, GameMatcher.HealthBarUI));
    }

    public void Execute()
    {
        foreach (var entity in _healthBarEntities.GetEntities())
        {
            var healthBarUI = entity.healthBarUI;
            healthBarUI.FillBar.value = Mathf.Lerp(healthBarUI.FillBar.value, entity.health.Value / entity.maxHealth.Value, Time.deltaTime * 7);
        }
    }
}
