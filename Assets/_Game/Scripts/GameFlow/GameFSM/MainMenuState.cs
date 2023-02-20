using System.Threading;
using _Game.Data;
using _Game.Flow;
using Game.Common;
using UIFramework.Components.NavBar;
using UnityEngine;
using VContainer.Unity;

public class MainMenuState : FSMState, IPostStartable
{
    private readonly AutoInjectFactory _autoInjectFactory;
    private readonly RewardsUIFeedbackService _rewardsUIFeedbackService;
    private readonly CommandQueueService _commandQueueService;
    private readonly PlayerData _playerData;
    private readonly MainMenuCommandQueue _commandQueue;

    private CancellationTokenSource _cancellationTokenSource;

    public MainMenuState(AutoInjectFactory autoInjectFactory,
        RewardsUIFeedbackService rewardsUIFeedbackService,
        CommandQueueService commandQueueService,
        PlayerData playerData)
    {
        _autoInjectFactory = autoInjectFactory;
        _rewardsUIFeedbackService = rewardsUIFeedbackService;
        _commandQueueService = commandQueueService;
        _playerData = playerData;
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

        _cancellationTokenSource = new CancellationTokenSource();
        _commandQueueService.ExecuteCommandQueue<MainMenuCommandQueue>(_cancellationTokenSource.Token);
    }

    internal override void OnExit()
    {
        _cancellationTokenSource.Cancel();
        _uiFrame.Close<NavBar>();
        _uiFrame.Close<CurrencyPanel>();
    }


    public void PostStart() //On Start CommandQueues could be not ready yet
    {
        _playerData.TotalExpPresented = _playerData.TotalExp;
        _commandQueueService.AddCommand<MainMenuCommandQueue>(new DelayCommand(1f));

        _commandQueueService.AddCommand<MainMenuCommandQueue>(
            new SyncPresentedExpCommand(
                _rewardsUIFeedbackService,
                _playerData, 
                _uiFrame.UICamera.ViewportToWorldPoint(new Vector3(0.5f, 0.2f))));
    }
}