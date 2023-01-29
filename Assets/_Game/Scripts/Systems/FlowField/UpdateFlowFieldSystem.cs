using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class UpdateFlowFieldSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private IGroup<GameEntity> _flowFieldGroup;
    private IGroup<GameEntity> _moversGroup;
    private IGroup<GameEntity> _tempObstaclesGroup;

    public UpdateFlowFieldSystem(Contexts contexts)
    {
        _contexts = contexts;

        _flowFieldGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowField));
        _moversGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowFieldMover, GameMatcher.Transform, GameMatcher.Radius));
        _tempObstaclesGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowFieldTemporaryObstacle, GameMatcher.Position, GameMatcher.Timer));
    }
    
    public void Execute()
    {
        if (_flowFieldGroup.count == 0) return;
        
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

            var repulsionSize = Mathf.RoundToInt(e.radius.Value / fieldSettings.CellSize);
            
            for (int i = -repulsionSize; i <= repulsionSize; i++)
            {
                for (int j = -repulsionSize; j <= repulsionSize; j++)
                {
                    int dist = Mathf.Abs(i) + Mathf.Abs(j) + 1;
                    if (dist > repulsionSize)
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
                    float dist = Mathf.Sqrt(i * i + j * j);
                    if (dist > e.flowFieldTemporaryObstacle.Radius)
                        continue;

                    var indexX = x + i;
                    var indexY = y + j;

                    if (flowField.IsIndexValid(indexX, indexY) && flowField.BackField[indexX][indexY] != int.MaxValue)
                    {
                        flowField.BackField[indexX][indexY] += Mathf.RoundToInt(e.flowFieldTemporaryObstacle.InitialWeight / (dist + 1));
                    }
                }
            }
        }
    }
}