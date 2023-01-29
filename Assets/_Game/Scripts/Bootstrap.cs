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
    
    void Start()
    {
        Application.targetFrameRate = 60;
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
                
                .Add(new PlayerUpdateSystem(contexts))
                .Add(new PlayerShooterDirectionUpdateSystem(contexts))
                
                .Add(new TimerSystem(contexts))
                
                .Add(new CreateViewSystem(contexts))
                
                .Add(new CameraControlSystem(contexts))
                
                .Add(new LevelStageProgressSystem(contexts))
                
                .Add(new EnemySpawnersUpdateSystem(contexts))
                .Add(new EnemySpawnSystem(contexts))
            
                .Add(new FlowFieldFeature(contexts))
                
                .Add(new LaserVisualizationSystem(contexts))
                
                .Add(new ShootingFeature(contexts))
                
                .Add(new MovementSystem(contexts))
                .Add(new RotationSystem(contexts))
                .Add(new RagdollUpdateSystem(contexts))
                
                .Add(new LandingExplosionSystem(contexts))
                
                .Add(new ExplosionSystem(contexts))
                
                .Add(new UpdateViewSystem(contexts))
                .Add(new DebugExplosionVisualizationSystem(contexts))
            
                .Add(new DebugFeature(contexts))
                
                .Add(new FloatDamageSystem(contexts))
                .Add(new DamageSystem(contexts))

                .Add(new RagdollCreationSystem(contexts))//should be after damage system
                
                .Add(new AutoDestructionSystem(contexts))
                .Add(new DestroyViewSystem(contexts))
                .Add(new MultiDestroySystem(contexts))
                
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
