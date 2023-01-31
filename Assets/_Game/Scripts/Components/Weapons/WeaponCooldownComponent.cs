using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, FlagPrefix("has")]
public class WeaponCooldownComponent : IComponent
{
    public static void StartWeaponCooldown(GameEntity e, float cooldown, IContext<GameEntity> context)
    {
        var timerEntity = context.CreateEntity();
        timerEntity.AddTimer(cooldown);
        timerEntity.AddEntityRef(e.id.Value);
        timerEntity.hasWeaponCooldown = true;
        e.hasWeaponCooldown = true;
    }
}

