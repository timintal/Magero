using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Data;
using _Game.Flow;
using Entitas;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

public class EcsBootstrap : MonoBehaviour, IDisposable
{
    [SerializeField] private List<MonoEntityLink> _sceneEntities = new();
    
    [Inject] private GameSetup _gameSetup;
    [Inject] private GameSceneReferences _gameSceneReferences;
    [Inject] private PoolService _poolService;
    [Inject] private Contexts _contexts;
    [Inject] private IUnitHealthProvider _unitHealthProvider;
    [Inject] private IUnitDamageProvider _unitDamageProvider;
    [Inject] private IUnitMovementSpeedProvider _movementSpeedProvider ;
    [Inject] private PlayerData _playerData;
    [Inject] private GameFSM _gameFsm;

    private Systems _systems;
    
    
   
    
    void Start()
    {
        _contexts.game.SetGameSetup(_gameSetup);
        _contexts.game.SetGameSceneReferences(_gameSceneReferences);
        
        _contexts.SubscribeId();
        _systems = CreateSystems(_contexts);
        _systems.Initialize();

        foreach (var entityLink in _sceneEntities)
        {
            entityLink.CreateEntity(_contexts);
        }
    }
    
    private Systems CreateSystems(Contexts contexts)
    {
        return new Feature("Game")
                .Add(new PlayerInitializeSystem(contexts, _playerData))
                .Add(new LevelStageActivationSystem(contexts))
                
                .Add(new ColliderCacheSystem(contexts))
                
                .Add(new UserInputSystem(contexts))
                
                .Add(new TimerSystem(contexts))
                
                //Reset
                .Add(new SpeedResetSystem(contexts))
                
                //effects
                .Add(new StunMovementSystem(contexts))
                .Add(new ApplyStunSystem(contexts))
                .Add(new SpeedModifierZoneUpdateSystem(contexts))
                
                .Add(new PlayerUpdateSystem(contexts))
                .Add(new PlayerShooterDirectionUpdateSystem(contexts))
                

                .Add(new CameraControlSystem(contexts))
                
                .Add(new LevelStageProgressSystem(contexts))
                
                .Add(new EnemySpawnersUpdateSystem(contexts))
                .Add(new EnemySpawnSystem(contexts, _unitHealthProvider, _movementSpeedProvider, _unitDamageProvider, _playerData))
            
                .Add(new FlowFieldFeature(contexts))

                .Add(new WeaponFeature(contexts))
                
                .Add(new ForcedMovementSystem(contexts))
                .Add(new MovementSystem(contexts))
                .Add(new RotationSystem(contexts))
                .Add(new RagdollUpdateSystem(contexts))

                .Add(new SummonUpdateSystem(contexts))
                
                .Add(new ExplosionSystem(contexts))
                
                .Add(new PlayerDamageSystem(contexts))
                
                .Add(new DebugFeature(contexts))
                
                .Add(new DamageSystem(contexts))

                .Add(new RagdollCreationSystem(contexts))//should be after damage system
                
                .Add(new AutoDestructionSystem(contexts))
                .Add(new SpawnOnDestroyVisualSystem(contexts))
                .Add(new ReturnToPoolSystem(contexts))
                .Add(new MultiDestroySystem(contexts))
                
                .Add(new PlayerHealthSystem(contexts))
                .Add(new LevelFinishSystem(contexts, _gameFsm))
                
                //view
                .Add(new AnimatorSpeedSyncSystem(contexts))
                .Add(new CreateViewSystem(contexts, _poolService))
                
                .Add(new WeaponBeamVisualizationSystem(contexts))
                .Add(new WeaponHitPointFxUpdateSystem(contexts))
                
                .Add(new ArmAnimationBeamSystem(contexts))
                .Add(new ArmAnimationCastSystem(contexts))
                .Add(new ArmAnimationSystemForProjectiles(contexts))
                
                .Add(new ExplosionVisualizationSystem(contexts))
                
                
                .Add(new UpdateViewSystem(contexts))
                .Add(new HealthBarSystem(contexts))
                
                //Cleanup
                .Add(new CleanupComponentSystem<DamageSourcePositionComponent>(contexts.game))
            ;
    }

    private void Update()
    {
        if (_gameFsm.CurrentState.GetType() == typeof(GameplayState))
            _systems.Execute();
    }

    private void LateUpdate()
    {
        _systems.Cleanup();
    }

    private void OnApplicationQuit()
    {
        Dispose();
    }

    private void OnDestroy()
    {
        Dispose();
    }

    public void Dispose()
    {
        _systems.DeactivateReactiveSystems();
        _systems.TearDown();
        _contexts.Reset();
    }

    [Button]
    void CollectSceneEntities()
    {
        _sceneEntities.Clear();
        foreach (var rootGameObject in gameObject.scene.GetRootGameObjects())
        {
            _sceneEntities.AddRange(rootGameObject.GetComponentsInChildren<MonoEntityLink>().ToList().FindAll(x=>x.CreateOnStart));
        }
        
    }
}
