using _Game.Flow;

public class GameOverState : FSMState
{
    internal override void OnEnter()
    {
        _uiFrame.Open<GameOverScreen>();
    }

    internal override void OnExit()
    {
        _uiFrame.Close<GameOverScreen>();
    }
}