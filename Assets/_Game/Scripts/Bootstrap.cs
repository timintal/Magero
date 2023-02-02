using System.Collections.Generic;
using System.Linq;
using Entitas;
using Sirenix.OdinInspector;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameSetup _gameSetup;
    [SerializeField] private GameSceneReferences _gameSceneReferences;
    [SerializeField] private List<MonoEntityLink> _sceneEntities = new();
    
    private Systems _systems;
    private PoolService _poolService;
    
    void Start()
    {
        Application.targetFrameRate = 60;

        _poolService = new PoolService();
        
        Contexts contexts = new Contexts();
        contexts.game.SetGameSetup(_gameSetup);
        contexts.game.SetGameSceneReferences(_gameSceneReferences);
        
        contexts.SubscribeId();
        _systems = CreateSystems(contexts);
        _systems.Initialize();

        foreach (var entityLink in _sceneEntities)
        {
            entityLink.CreateEntity(contexts);
        }
    }
    
    private Systems CreateSystems(Contexts contexts)
    {
        return new Feature("Game")
                .Add(new PlayerInitializeSystem(contexts))
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
                .Add(new EnemySpawnSystem(contexts))
            
                .Add(new FlowFieldFeature(contexts))

                .Add(new WeaponFeature(contexts))
                
                .Add(new ForcedMovementSystem(contexts))
                .Add(new MovementSystem(contexts))
                .Add(new RotationSystem(contexts))
                .Add(new RagdollUpdateSystem(contexts))

                .Add(new ExplosionSystem(contexts))
                
                .Add(new PlayerDamageSystem(contexts))
                
                .Add(new DebugFeature(contexts))
                
                .Add(new DamageSystem(contexts))

                .Add(new RagdollCreationSystem(contexts))//should be after damage system
                
                .Add(new AutoDestructionSystem(contexts))
                .Add(new ReturnToPoolSystem(contexts))
                .Add(new MultiDestroySystem(contexts))
                
                //view
                .Add(new AnimatorSpeedSyncSystem(contexts))
                .Add(new CreateViewSystem(contexts, _poolService))
                
                .Add(new WeaponBeamVisualizationSystem(contexts))
                .Add(new WeaponHitPointFxUpdateSystem(contexts))
                .Add(new ArmAnimationBeamSystem(contexts))
                
                .Add(new ExplosionVisualizationSystem(contexts))
                
                .Add(new ArmAnimationSystemForProjectiles(contexts))
                
                .Add(new UpdateViewSystem(contexts))
                .Add(new HealthBarSystem(contexts))
                
                //Cleanup
                .Add(new CleanupComponentSystem<DamageSourcePositionComponent>(contexts.game))
            ;
    }

    private void Update()
    {
        _systems.Execute();
    }

    private void LateUpdate()
    {
        _systems.Cleanup();
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
