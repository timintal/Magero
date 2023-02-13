using _Game.Data;
using _Game.Flow;
using Game.Common;
using Game.Config.Model;
using Magero.UIFramework;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

public class InitialLifetimeScope : LifetimeScope
{
    [SerializeField] private GameSetup _gameSetup;
    [SerializeField] private UISettings _uiSettings;
    [SerializeField] private Camera _uiCamera;
    
    private UIFrame _uiFrame;
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_gameSetup);

        builder.Register<AutoInjectFactory>(Lifetime.Scoped).AsSelf();
        
        builder.RegisterInstance(_uiCamera);

        builder.Register<WeaponControlService>(Lifetime.Singleton);

        builder.Register<GameConfig>(Lifetime.Singleton);
        builder.RegisterEntryPoint<EnemyStatsConfig>();
        
        RegisterServices(builder);

        RegisterUI(builder);
        RegisterFsm(builder);
        RegisterGameSequences(builder);
        RegisterData(builder);
    }

    private static void RegisterServices(IContainerBuilder builder)
    {
        builder.Register<ExpForUpgradeService>(Lifetime.Singleton);
        builder.Register<PassiveIncomeService>(Lifetime.Singleton);
    }

    private void RegisterUI(IContainerBuilder builder)
    {
        _uiFrame = _uiSettings.BuildUIFrame();
        SceneManager.MoveGameObjectToScene(_uiFrame.gameObject, gameObject.scene);
        builder.RegisterComponent(_uiFrame).AsSelf();
        
        _uiFrame.AddEventForAllScreens(OnScreenEvent.Created, UiFrameOnOnScreenCreated);
    }
    
    private void UiFrameOnOnScreenCreated(UIScreenBase screen)
    {
        Container.InjectGameObject(screen.gameObject);
    }
    
    void RegisterGameSequences(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<InjectableGameFlowService>().As<GameFlowService>();
    }

    void RegisterFsm(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<InjectableGameFSM>().As<GameFSM>();

        builder.Register<FSMState, MainMenuState>(Lifetime.Singleton);
        builder.Register<FSMState, GameplayState>(Lifetime.Singleton);
        builder.Register<FSMState, LevelWonState>(Lifetime.Singleton);
        builder.Register<FSMState, GameOverState>(Lifetime.Singleton);
        builder.Register<FSMState, UnloadGameplayState>(Lifetime.Singleton);
    }

    private static void RegisterData(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<DataManager>().AsSelf();
        builder.Register<IPersistentDataHandler, PlayerPrefsDataHandler>(Lifetime.Singleton);
        builder.Register<PlayerData>(Lifetime.Singleton).As<PersistentDataBase>().AsSelf();
        builder.Register<WeaponData>(Lifetime.Singleton).As<PersistentDataBase>().AsSelf();
        builder.Register<PassiveIncomeData>(Lifetime.Singleton).As<PersistentDataBase>().AsSelf();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        _uiFrame.Initialize(_uiCamera);

        Container.Resolve<GameFSM>().GoTo<MainMenuState>();
        Container.Resolve<GameConfig>().Init(null);
        
        Container.Resolve<ExpForUpgradeService>().Init();
        
        AddCheats();
    }

    private void AddCheats()
    {
        var cheats = new GeneralCheats();
        Container.Inject(cheats);
        SRDebug.Instance.AddOptionContainer(cheats);
    }
}
