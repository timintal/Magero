using _Game.Data;
using _Game.Flow;

public class LevelWonState : FSMState
{
    private readonly PlayerData _playerData;

    public LevelWonState(PlayerData playerData)
    {
        _playerData = playerData;
    }
    
    internal override void OnEnter()
    {
        _playerData.Level += 1;
        _uiFrame.Open<LevelFinishedScreen>();
    }

    internal override void OnExit()
    {
        _uiFrame.Close<LevelFinishedScreen>();
    }
}