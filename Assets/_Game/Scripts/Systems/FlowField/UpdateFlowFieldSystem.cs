using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class UpdateFlowFieldSystem : IExecuteSystem, IInitializeSystem
{
    private readonly Contexts _contexts;
    private IGroup<GameEntity> _targetsGroup;
    private IGroup<GameEntity> _flowFieldGroup;
    private IGroup<GameEntity> _moversGroup;
    private List<int> _cellsToCheck;
    private IGroup<GameEntity> _tempObstaclesGroup;

    public UpdateFlowFieldSystem(Contexts contexts)
    {
        _contexts = contexts;

        _targetsGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowFieldTarget, GameMatcher.Transform));
        _flowFieldGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowField));
        _moversGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowFieldMover, GameMatcher.Transform));
        _tempObstaclesGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowFieldTemporaryObstacle, GameMatcher.Position, GameMatcher.Timer));
        _cellsToCheck = new List<int>(8192);
    }
    
    public void Execute()
    {
        var flowField = _flowFieldGroup.GetSingleEntity().flowField;
        flowField.ResetField();

        IncludeMoversIntoFlowField(flowField);
        UpdateFlowFieldObstacles(flowField);
        
        flowField.SwapFields();
    }

    private void IncludeMoversIntoFlowField(FlowFieldComponent flowField)
    {
        var fieldSettings = _contexts.game.gameSetup.value.FlowFieldSettings;
        
        foreach (var e in _moversGroup.GetEntities())
        {
            var pos = e.position.Value;
            var (x, y) = flowField.GetIndex(pos);

            for (int i = -fieldSettings.MoverRepulsionSize; i <= fieldSettings.MoverRepulsionSize; i++)
            {
                for (int j = -fieldSettings.MoverRepulsionSize; j <= fieldSettings.MoverRepulsionSize; j++)
                {
                    int dist = Mathf.Abs(i) + Mathf.Abs(j) + 1;
                    if (dist > fieldSettings.MoverRepulsionSize)
                        continue;

                    var indexX = x + i;
                    var indexY = y + j;

                    if (flowField.IsIndexValid(indexX, indexY) && flowField.BackField[indexX][indexY] != int.MaxValue)
                    {
                        flowField.BackField[indexX][indexY] += fieldSettings.MoverRepulsionValue / dist;
                    }
                }
            }
        }
    }
    
    private void UpdateFlowFieldObstacles(FlowFieldComponent flowField)
    {
        var fieldSettings = _contexts.game.gameSetup.value.FlowFieldSettings;
        
        foreach (var e in _tempObstaclesGroup.GetEntities())
        {
            if (e.isTimerCompleted)
            {
                e.isDestroyed = true;
                continue;
            }
            
            var pos = e.position.Value;
            var (x, y) = flowField.GetIndex(pos);

            for (int i = -e.flowFieldTemporaryObstacle.Radius; i <= e.flowFieldTemporaryObstacle.Radius; i++)
            {
                for (int j = -e.flowFieldTemporaryObstacle.Radius; j <= e.flowFieldTemporaryObstacle.Radius; j++)
                {
                    int dist = Mathf.Abs(i) + Mathf.Abs(j) + 1;
                    if (dist > e.flowFieldTemporaryObstacle.Radius)
                        continue;

                    var indexX = x + i;
                    var indexY = y + j;

                    if (flowField.IsIndexValid(indexX, indexY) && flowField.BackField[indexX][indexY] != int.MaxValue)
                    {
                        flowField.BackField[indexX][indexY] += e.flowFieldTemporaryObstacle.InitialWeight / dist;
                    }
                }
            }
        }
    }

    private void UpdateFlowFieldForTarget(GameEntity targetEntity, FlowFieldComponent flowField,
        FlowFieldSettings fieldSettings)
    {
        var targetPosition = targetEntity.transform.Transform.position;

        var (initX, initY) = flowField.GetIndex(targetPosition);
        for (int i = -5; i < 10; i++)
        {
            if (flowField.IsIndexValid(initX + i, initY))
            {
                _cellsToCheck.Add(PackedIndex(initX + i, initY));
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
        
            // if (currValue >= fieldSettings.MaxCalculationDistance) continue;
        
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
                            _cellsToCheck.Add(PackedIndex(currX + i, currY + j));
                        }
                    }
                }
            }
        }
    }


    private static int PackedIndex(int x, int y)
    {
        int plainIndex = x;
        plainIndex <<= 16;
        plainIndex += y;
        return plainIndex;
    }

    public void Initialize()
    {
        var levelToLoad = _contexts.game.gameSceneReferences.value.LevelToLoad;
        
        var flowFieldEntity = _contexts.game.CreateEntity();

        int[][] flowField = new int[levelToLoad.width][];
        int[][] currentField = new int[levelToLoad.width][];
        int[][] backFiled = new int[levelToLoad.width][];
        
        for (int i = 0; i < flowField.Length; i++)
        {
            flowField[i] = new int[levelToLoad.height];
            currentField[i] = new int[levelToLoad.height];
            backFiled[i] = new int[levelToLoad.height];
        }

        var gameSetup = _contexts.game.gameSetup.value;
        
        for (int i = 0; i < levelToLoad.width; i++)
        {
            for (int j = 0; j < levelToLoad.height; j++)
            {
                flowField[i][j] = gameSetup.FlowFieldSettings.MaxCalculationDistance;
                currentField[i][j] = gameSetup.FlowFieldSettings.MaxCalculationDistance;
                backFiled[i][j] = gameSetup.FlowFieldSettings.MaxCalculationDistance;
            }
        }

        var initialPos = levelToLoad.transform.position;
        
        foreach (var obstacle in levelToLoad.obstacles)
        {
            for (int i = 0; i < obstacle.width; i++)
            {
                for (int j = 0; j < obstacle.height; j++)
                {
                    flowField[obstacle.indexX + i][obstacle.indexY + j] = int.MaxValue;
                    currentField[obstacle.indexX + i][obstacle.indexY + j] = int.MaxValue;
                    backFiled[obstacle.indexX + i][obstacle.indexY + j] = int.MaxValue;
                }
            }
        }

        flowFieldEntity.AddFlowField(initialPos, levelToLoad.cellSize, flowField, currentField, backFiled);
        
        foreach (var crowdTarget in levelToLoad.CrowdTargets)
        {
            var targetEntity = _contexts.game.CreateEntity();
            targetEntity.AddFlowFieldTarget(0);
            targetEntity.AddTransform(crowdTarget);
            UpdateFlowFieldForTarget(targetEntity, flowFieldEntity.flowField, gameSetup.FlowFieldSettings);
        }
        
        flowFieldEntity.flowField.CopyField(flowFieldEntity.flowField.BackField, flowFieldEntity.flowField.LevelField);
        
    }
}