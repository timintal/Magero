using System.Collections.Generic;
using Entitas;

public class WeaponCooldownSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public WeaponCooldownSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.TimerCompleted.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasWeaponCooldown && entity.hasEntityRef;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var weapon = _contexts.game.GetEntityWithId(e.entityRef.EntityId);
            weapon.hasWeaponCooldown = false;
        }
    }
}