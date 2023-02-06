using Entitas;

public class PlayerHealthSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private IGroup<GameEntity> _playerGroup;

    public PlayerHealthSystem(Contexts contexts)
    {
        _contexts = contexts;
        _playerGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.Health));
    }
    public void Execute()
    {
        foreach (var e in _playerGroup.GetEntities())
        {
            if (e.health.Value <= 0)
            {
                var gameEntity = _contexts.game.CreateEntity();
                gameEntity.AddLevelFinished(false);
            }
        }
    }
}