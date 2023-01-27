using Entitas;

public class PlayerUpdateSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _playerGroup;
    private IGroup<InputEntity> _userInputGroup;

    public PlayerUpdateSystem(Contexts contexts)
    {
        _contexts = contexts;
        _playerGroup = contexts.game.GetGroup(GameMatcher.Player);
        _userInputGroup = contexts.input.GetGroup(InputMatcher.UserInput);
    }

    public void Execute()
    {
        var isFirePressed = _userInputGroup.count == 0 || _userInputGroup.GetSingleEntity().userInput.IsFirePressed;
        foreach (var e in _playerGroup.GetEntities())
        {
            e.isWeaponDisabled = !isFirePressed;
        }
    }
}