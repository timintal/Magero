using System.Collections.Generic;
using Cinemachine;
using Entitas;
using UnityEngine;


public class LevelStageActivationSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    private IGroup<GameEntity> _levelGroup;
    private IGroup<GameEntity> _groundEnemyFlowFieldGroup;
    private IGroup<GameEntity> _flyingEnemyFlowFieldGroup;
    private IGroup<GameEntity> _summonFlowFieldGroup;
    
    private List<int> _cellsToCheck = new List<int>(8192);
    private IGroup<GameEntity> _cameraGroup;

    public LevelStageActivationSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _levelGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Level));
        
        _groundEnemyFlowFieldGroup =
            contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowField, GameMatcher.GroundEnemyFlowField));
        _flyingEnemyFlowFieldGroup =
            contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowField, GameMatcher.FlyingEnemyFlowField));
        _summonFlowFieldGroup =
            contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowField, GameMatcher.SummonFlowField));
        
        _cameraGroup = contexts.game.GetGroup(GameMatcher.Camera);
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.CurrentLevelStage.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCurrentLevelStage;
    }

    protected override void Execute(List<GameEntity> entities)
    {
#if UNITY_EDITOR
        if (entities.Count > 1) Debug.LogWarning("You should have 1 level stage index entity");
#endif

        foreach (var e in entities)
        {
            var currentLevelStageIndex = e.currentLevelStage.Index;
            var level = _levelGroup.GetSingleEntity();
            var levelStages = level.level.Stages;
            if (levelStages.Length > currentLevelStageIndex)
            {
                GenerateGroundEnemyFlowField(levelStages[currentLevelStageIndex]);
                GenerateFlyingEnemyFlowField(levelStages[currentLevelStageIndex]);
                GenerateSummonFlowField(levelStages[currentLevelStageIndex]);
                
                ReplaceCamera(levelStages[currentLevelStageIndex].StageCamera);
                CreateEnemySpawners(levelStages[currentLevelStageIndex]);
            }
            else
            {
                level.ReplaceLevelFinished(true);
            }
        }
    }

    void CreateEnemySpawners(LevelStage stage)
    {
        foreach (var enemySpawner in stage.EnemySpawners)
        {
            enemySpawner.CreateEntity(_contexts);
        }
    }

    void ReplaceCamera(CinemachineVirtualCamera camera)
    {
        GameEntity cameraEntity = null;

        if (_cameraGroup.count == 0)
        {
            cameraEntity = _contexts.game.CreateEntity();
        }
        else
        {
            cameraEntity = _cameraGroup.GetSingleEntity();
            var cinemachineVirtualCamera = cameraEntity.transform.Transform.GetComponent<CinemachineVirtualCamera>();
            if (cinemachineVirtualCamera != null)
            {
                cinemachineVirtualCamera.enabled = false;
            }
        }

        var gameSetup = _contexts.game.gameSetup.value;

        cameraEntity.ReplaceCamera(
            camera.transform.rotation,
            gameSetup.CameraSettings.HorizontalBounds,
            gameSetup.CameraSettings.VerticalBounds,
            gameSetup.CameraSettings.RotationSpeed);

        cameraEntity.ReplaceTransform(camera.transform);
        camera.enabled = true;
    }

    private void GenerateGroundEnemyFlowField(LevelStage levelToLoad)
    {
        var gameSetup = _contexts.game.gameSetup.value;

        if (_groundEnemyFlowFieldGroup.count > 0)
        {
            var prev = _groundEnemyFlowFieldGroup.GetSingleEntity();
            prev.Destroy();
        }
        
        var flowFieldEntity = CreateFlowFieldEntity(levelToLoad, gameSetup);
        flowFieldEntity.isGroundEnemyFlowField = true;

        foreach (var crowdTarget in levelToLoad.CrowdTargets)
        {
            HelperFunctions.UpdateFlowFieldForTarget(crowdTarget.position, flowFieldEntity.flowField, gameSetup.FlowFieldSettings, 4f, _cellsToCheck);
        }

        flowFieldEntity.flowField.CopyField(flowFieldEntity.flowField.BackField, flowFieldEntity.flowField.LevelField);

        AddObstaclesRepulsion(levelToLoad, gameSetup, flowFieldEntity);
    }

    private void GenerateFlyingEnemyFlowField(LevelStage levelToLoad)
    {
        var gameSetup = _contexts.game.gameSetup.value;

        if (_flyingEnemyFlowFieldGroup.count > 0)
        {
            var prev = _flyingEnemyFlowFieldGroup.GetSingleEntity();
            prev.Destroy();
        }
        
        var flowFieldEntity = CreateFlowFieldEntity(levelToLoad, gameSetup, 1,true);
        flowFieldEntity.isFlyingEnemyFlowField = true;
        
        foreach (var crowdTarget in levelToLoad.CrowdTargets)
        {
            HelperFunctions.UpdateFlowFieldForTarget(crowdTarget.position, flowFieldEntity.flowField, gameSetup.FlowFieldSettings, 8f, _cellsToCheck);
        }

        flowFieldEntity.flowField.CopyField(flowFieldEntity.flowField.BackField, flowFieldEntity.flowField.LevelField);

        AddObstaclesRepulsion(levelToLoad, gameSetup, flowFieldEntity);
    }
    
    
    private void GenerateSummonFlowField(LevelStage levelToLoad)
    {
        var gameSetup = _contexts.game.gameSetup.value;

        if (_summonFlowFieldGroup.count > 0)
        {
            var prev = _summonFlowFieldGroup.GetSingleEntity();
            prev.Destroy();
        }
        
        var flowFieldEntity = CreateFlowFieldEntity(levelToLoad, gameSetup, 4);
        flowFieldEntity.isSummonFlowField = true;
        
    }

    private static void AddObstaclesRepulsion(LevelStage levelToLoad, GameSetup gameSetup, GameEntity flowFieldEntity)
    {
        var obstacleRepulsionSize = gameSetup.FlowFieldSettings.ObstacleRepulsionSize;
        foreach (var obstacle in levelToLoad.obstacles)
        {
            for (int i = -obstacleRepulsionSize; i < obstacle.width + obstacleRepulsionSize; i++)
            {
                for (int j = -obstacleRepulsionSize; j < obstacle.height + obstacleRepulsionSize; j++)
                {
                    var xIndex = obstacle.indexX + i;
                    var yIndex = obstacle.indexY + j;

                    if (xIndex < 0 || yIndex < 0 || xIndex >= levelToLoad.width || yIndex >= levelToLoad.height)
                        continue;

                    Vector2 distanceFromObstacle = Vector2.zero;

                    if (i < 0) distanceFromObstacle.x = Mathf.Abs(i);
                    if (j < 0) distanceFromObstacle.y = Mathf.Abs(j);
                    if (i > obstacle.width - 1) distanceFromObstacle.x = i - obstacle.width + 1;
                    if (j > obstacle.height - 1) distanceFromObstacle.y = j - obstacle.height + 1;

                    float magn = distanceFromObstacle.magnitude;
                    if (magn > 0)
                    {
                        flowFieldEntity.flowField.LevelField[xIndex][yIndex] +=
                            Mathf.RoundToInt(gameSetup.FlowFieldSettings.ObstacleRepulsionValue / magn);
                    }
                }
            }
        }
    }

    private GameEntity CreateFlowFieldEntity(LevelStage levelToLoad, GameSetup gameSetup, int cellSizeFactor = 1, bool isForFlyingEnemies = false)
    {
        GameEntity flowFieldEntity = null;
        int[][] levelField;
        int[][] currentField;
        int[][] backFiled;

        flowFieldEntity = _contexts.game.CreateEntity();

        int finalWidth =  Mathf.RoundToInt((float)levelToLoad.width / cellSizeFactor);
        int finalHeight = Mathf.RoundToInt((float)levelToLoad.height / cellSizeFactor);
        float cellSize = cellSizeFactor * gameSetup.FlowFieldSettings.CellSize;
        
        levelField = new int[finalWidth][];
        currentField = new int[finalWidth][];
        backFiled = new int[finalWidth][];

        for (int i = 0; i < levelField.Length; i++)
        {
            levelField[i] = new int[finalHeight];
            currentField[i] = new int[finalHeight];
            backFiled[i] = new int[finalHeight];
        }


        for (int i = 0; i < finalWidth; i++)
        {
            for (int j = 0; j < finalHeight; j++)
            {
                levelField[i][j] = gameSetup.FlowFieldSettings.MaxCalculationDistance;
                currentField[i][j] = gameSetup.FlowFieldSettings.MaxCalculationDistance;
                backFiled[i][j] = gameSetup.FlowFieldSettings.MaxCalculationDistance;
            }
        }

        var initialPos = levelToLoad.FlowFieldInitialPos;

        foreach (var obstacle in levelToLoad.obstacles)
        {
            if (!obstacle.affectFlying && isForFlyingEnemies) continue;

            int width = Mathf.RoundToInt((float)obstacle.width / cellSizeFactor);
            int height = Mathf.RoundToInt((float)obstacle.height / cellSizeFactor);
            var obstacleIndexX = Mathf.RoundToInt((float)obstacle.indexX / cellSizeFactor);
            var obstacleIndexY = Mathf.RoundToInt((float)obstacle.indexY / cellSizeFactor);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    levelField[obstacleIndexX + i][obstacleIndexY + j] = int.MaxValue;
                    currentField[obstacleIndexX + i][obstacleIndexY + j] = int.MaxValue;
                    backFiled[obstacleIndexX + i][obstacleIndexY + j] = int.MaxValue;
                }
            }
        }

        flowFieldEntity.ReplaceFlowField(initialPos, cellSize, levelField, currentField, backFiled);
        return flowFieldEntity;
    }

    
}