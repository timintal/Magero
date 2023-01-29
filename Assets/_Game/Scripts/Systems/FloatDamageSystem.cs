using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class FloatDamageSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;

    public FloatDamageSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.FloatDamage));;
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasFloatDamage && entity.floatDamage.Value > 1;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            int totalDamage = Mathf.FloorToInt(e.floatDamage.Value);
            e.ReplaceFloatDamage(e.floatDamage.Value - totalDamage);
            if (e.hasDamage)
            {
                totalDamage += e.damage.Damage;
            }
            
            e.ReplaceDamage(totalDamage);
        }
    }
}