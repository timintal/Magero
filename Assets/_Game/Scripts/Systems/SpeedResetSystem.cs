using Entitas;

public class SpeedResetSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _speedGroup;

    public SpeedResetSystem(Contexts contexts)
    {
        _contexts = contexts;
        _speedGroup = contexts.game.GetGroup(GameMatcher.Speed);
    }

    public void Execute()
    {
        foreach (var e in _speedGroup.GetEntities())
        {
            e.ReplaceSpeed(e.speed.BaseValue, e.speed.BaseValue);
        }
    }
}