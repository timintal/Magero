using _Game.Data;
using _Game.Flow;
using Cysharp.Threading.Tasks;
using Game.Common;
using Game.Config.Model;
using Magero.UIFramework.Components.NavBar;

public class MainMenuState : FSMState
{
    private readonly AutoInjectFactory _autoInjectFactory;
    private readonly PlayerData _playerData;
    private readonly GameConfig _gameConfig;

    public MainMenuState(AutoInjectFactory autoInjectFactory, PlayerData playerData, GameConfig gameConfig)
    {
        _autoInjectFactory = autoInjectFactory;
        _playerData = playerData;
        _gameConfig = gameConfig;
    }

    internal override void OnEnter()
    {
        UpdatePlayerLevel();
        
        _uiFrame.Open<NavBar>(new NavBarProps
        {
            MainUI = _uiFrame,
            PreSelectIndex = 1,
            AutoInjectFactory = _autoInjectFactory
        });
        
        _uiFrame.Open<CurrencyPanel>();
    }

    internal override void OnExit()
    {
        _uiFrame.Close<NavBar>();
        _uiFrame.Close<CurrencyPanel>();
    }

    async void UpdatePlayerLevel()
    {
        await UniTask.WaitUntil(()=>_gameConfig.Initialised);
        var expModels = _gameConfig.GetConfigModel<ExpModel>();
        
        var expToNextLevel = expModels[_playerData.PlayerLevel.ToString()];

        while (_playerData.PlayerExp > expToNextLevel.Exp)
        {
            _playerData.PlayerExp -= expToNextLevel.Exp;
            _playerData.PlayerLevel += 1;
            expToNextLevel = expModels[_playerData.PlayerLevel.ToString()];
        }
    }
}