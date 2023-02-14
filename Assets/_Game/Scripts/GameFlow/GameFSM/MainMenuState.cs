using _Game.Flow;
using Game.Common;
using Magero.UIFramework.Components.NavBar;

public class MainMenuState : FSMState
{
    private readonly AutoInjectFactory _autoInjectFactory;

    public MainMenuState(AutoInjectFactory autoInjectFactory)
    {
        _autoInjectFactory = autoInjectFactory;
    }

    internal override void OnEnter()
    {
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

   
}