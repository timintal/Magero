using System;
using _Game.Data;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VContainer;
using VContainer.Unity;

public class GameplayLifetimeScope : LifetimeScope
{
    
    [SerializeField] private UnitStatsService _unitStatsService;
    [SerializeField] GameSceneReferences _gameSceneReferences;
    [SerializeField] private Camera _mainCamera;

    private GameplayCheats _gameplayCheats;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_gameSceneReferences);

        builder.Register<PoolService>(Lifetime.Singleton);
        builder.Register<Contexts>(Lifetime.Scoped);
        builder.RegisterInstance(_unitStatsService).AsImplementedInterfaces();

        SetupCamera();
    }

    private void SetupCamera()
    {
        var uiCamera = Parent.Container.Resolve<Camera>();
        _mainCamera.GetComponent<UniversalAdditionalCameraData>().cameraStack.Add(uiCamera);
    }

    private void Start()
    {
        _gameplayCheats = new GameplayCheats();
        Container.Inject(_gameplayCheats);
        SRDebug.Instance.AddOptionContainer(_gameplayCheats);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (SRDebug.Instance != null)
        {
            SRDebug.Instance.RemoveOptionContainer(_gameplayCheats);
        }
    }
}