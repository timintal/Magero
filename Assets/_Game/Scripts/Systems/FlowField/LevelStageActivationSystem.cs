using System.Collections.Generic;
using Cinemachine;
using Entitas;
using UnityEngine;


public class LevelStageActivationSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    private IGroup<GameEntity> _levelGroup;
    private IGroup<GameEntity> _flowFieldGroup;
    
    public LevelStageActivationSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _levelGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Level));
        _flowFieldGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowField));
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
                ActivateLevelStage(levelStages[currentLevelStageIndex]);
                ReplaceCamera(levelStages[currentLevelStageIndex].StageCamera);
                CreateEnemySpawners(levelStages[currentLevelStageIndex]);
            }
            else
            {
                level.isLevelFinished = true;
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

    private void ActivateLevelStage(LevelStage levelToLoad)
    {
        GameEntity flowFieldEntity = null;
        int[][] levelField;
        int[][] currentField;
        int[][] backFiled;


        if (_flowFieldGroup.count > 0)
        {
            flowFieldEntity = _flowFieldGroup.GetSingleEntity();
            flowFieldEntity.Destroy();
        }
        
        {
            flowFieldEntity = _contexts.game.CreateEntity();

            levelField = new int[levelToLoad.width][];
            currentField = new int[levelToLoad.width][];
            backFiled = new int[levelToLoad.width][];

            for (int i = 0; i < levelField.Length; i++)
            {
                levelField[i] = new int[levelToLoad.height];
                currentField[i] = new int[levelToLoad.height];
                backFiled[i] = new int[levelToLoad.height];
            }
        }

        var gameSetup = _contexts.game.gameSetup.value;

        for (int i = 0; i < levelToLoad.width; i++)
        {
            for (int j = 0; j < levelToLoad.height; j++)
            {
                levelField[i][j] = gameSetup.FlowFieldSettings.MaxCalculationDistance;
                currentField[i][j] = gameSetup.FlowFieldSettings.MaxCalculationDistance;
                backFiled[i][j] = gameSetup.FlowFieldSettings.MaxCalculationDistance;
            }
        }

        var initialPos = levelToLoad.FlowFieldInitialPos;

        foreach (var obstacle in levelToLoad.obstacles)
        {
            for (int i = 0; i < obstacle.width; i++)
            {
                for (int j = 0; j < obstacle.height; j++)
                {
                    levelField[obstacle.indexX + i][obstacle.indexY + j] = int.MaxValue;
                    currentField[obstacle.indexX + i][obstacle.indexY + j] = int.MaxValue;
                    backFiled[obstacle.indexX + i][obstacle.indexY + j] = int.MaxValue;
                }
            }
        }

        flowFieldEntity.ReplaceFlowField(initialPos, levelToLoad.cellSize, levelField, currentField, backFiled);

        foreach (var crowdTarget in levelToLoad.CrowdTargets)
        {
            UpdateFlowFieldForTarget(crowdTarget, flowFieldEntity.flowField, gameSetup.FlowFieldSettings);
        }

        flowFieldEntity.flowField.CopyField(flowFieldEntity.flowField.BackField, flowFieldEntity.flowField.LevelField);

        var obstacleRepulsionSize = gameSetup.FlowFieldSettings.ObstacleRepulsionSize;
        foreach (var obstacle in levelToLoad.obstacles)
        {
            for (int i = -obstacleRepulsionSize; i < obstacle.width + obstacleRepulsionSize; i++)
            {
                for (int j = -obstacleRepulsionSize; j < obstacle.height + obstacleRepulsionSize; j++)
                {
                    var xIndex = obstacle.indexX + i;
                    var yIndex = obstacle.indexY + j;
        
                    if (xIndex < 0 || yIndex < 0 || xIndex > levelToLoad.width || yIndex > levelToLoad.height)
                        continue;
        
                    Vector2 distanceFromObstacle = Vector2.zero;
        
                    if (i < 0) distanceFromObstacle.x = Mathf.Abs(i);
                    if (j < 0) distanceFromObstacle.y = Mathf.Abs(j);
                    if (i > obstacle.width - 1) distanceFromObstacle.x = i - obstacle.width + 1;
                    if (j > obstacle.height - 1) distanceFromObstacle.y = j - obstacle.height + 1;

                    float magn = distanceFromObstacle.magnitude;
                    if (magn > 0)
                    {
                        levelField[xIndex][yIndex] += Mathf.RoundToInt(gameSetup.FlowFieldSettings.ObstacleRepulsionValue / magn);
                    }
                }
            }
        }
    }

    private List<int> _cellsToCheck = new List<int>(8192);
    private IGroup<GameEntity> _cameraGroup;

    private void UpdateFlowFieldForTarget(Transform target, FlowFieldComponent flowField,
        FlowFieldSettings fieldSettings)
    {
        _cellsToCheck.Clear();
        var targetPosition = target.position;

        var (initX, initY) = flowField.GetIndex(targetPosition);
        for (int i = -5; i < 10; i++)
        {
            if (flowField.IsIndexValid(initX + i, initY))
            {
                _cellsToCheck.Add(HelperFunctions.PackedIndex(initX + i, initY));
                flowField.BackField[initX + i][initY] = 0;
            }
        }

        while (_cellsToCheck.Count > 0)
        {
            int currPlainIndex = _cellsToCheck[0];
            _cellsToCheck.RemoveAt(0);

            int currX = currPlainIndex >> 16;
            int currY = currPlainIndex - (currX << 16);

            int currValue = flowField.BackField[currX][currY];

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    int addValue = i != 0 && j != 0 ? fieldSettings.StepDiagonalWeight : fieldSettings.StepWeight;

                    if (flowField.IsIndexValid(currX + i, currY + j))
                    {
                        var val = flowField.BackField[currX + i][currY + j];
                        if (val > currValue + addValue && val != int.MaxValue)
                        {
                            flowField.BackField[currX + i][currY + j] = currValue + addValue;
                            _cellsToCheck.Add(HelperFunctions.PackedIndex(currX + i, currY + j));
                        }
                    }
                }
            }
        }
    }
}