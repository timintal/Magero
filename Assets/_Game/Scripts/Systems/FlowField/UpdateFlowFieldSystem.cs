using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class UpdateFlowFieldSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private IGroup<GameEntity> _groundEnemiesFlowFieldGroup;
    private IGroup<GameEntity> _flyingEnemiesFlowFieldGroup;
    private IGroup<GameEntity> _summonFlowFieldGroup;
    private IGroup<GameEntity> _tempObstaclesGroup;

    private List<int> _cellsBuffer;

    public UpdateFlowFieldSystem(Contexts contexts)
    {
        _contexts = contexts;

        _groundEnemiesFlowFieldGroup =
            _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowField, GameMatcher.GroundEnemyFlowField).NoneOf(GameMatcher.FieldUpdateCooldown));
        _flyingEnemiesFlowFieldGroup =
            _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowField, GameMatcher.FlyingEnemyFlowField).NoneOf(GameMatcher.FieldUpdateCooldown));
        _summonFlowFieldGroup =
            _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowField, GameMatcher.SummonFlowField).NoneOf(GameMatcher.FieldUpdateCooldown));
        _tempObstaclesGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.FlowFieldTemporaryObstacle,
            GameMatcher.Position, GameMatcher.Timer));

        _cellsBuffer = new List<int>(2048);
    }

    public void Execute()
    {
        if (_groundEnemiesFlowFieldGroup.count > 0)
        {
            var flowField = _groundEnemiesFlowFieldGroup.GetSingleEntity();
            flowField.flowField.ResetField();
            
            var movers = _contexts.game.GetEntitiesWithFlowFieldMover(flowField.id.Value);
            
            IncludeMoversIntoFlowField(flowField, movers);
            IncludeTempAvoidanceZones(flowField);
            flowField.flowField.SwapFields();

            AddUpdateCooldown(flowField);

            if (_summonFlowFieldGroup.count > 0)
            {
                var summonFlowField = _summonFlowFieldGroup.GetSingleEntity();
                summonFlowField.flowField.ResetField();
                
                var summonMovers = _contexts.game.GetEntitiesWithFlowFieldMover(summonFlowField.id.Value);
                // if (summonMovers.Count > 0)
                {
                    IncludeMoversIntoFlowField(summonFlowField, summonMovers);

                    foreach (var mover in movers)
                    {
                        HelperFunctions.UpdateFlowFieldForTarget(mover.position.Value, summonFlowField.flowField,
                            _contexts.game.gameSetup.value.FlowFieldSettings, 1, _cellsBuffer, 1000);
                    }

                    summonFlowField.flowField.SwapFields();
                    AddUpdateCooldown(summonFlowField);
                }
            }
        }

        if (_flyingEnemiesFlowFieldGroup.count > 0)
        {
            var flowField = _flyingEnemiesFlowFieldGroup.GetSingleEntity();
            flowField.flowField.ResetField();
            
            var movers = _contexts.game.GetEntitiesWithFlowFieldMover(flowField.id.Value);
            
            IncludeMoversIntoFlowField(flowField, movers);
            flowField.flowField.SwapFields();
            AddUpdateCooldown(flowField);
        }
        
    }

    private void AddUpdateCooldown(GameEntity flowField)
    {
        var timer = _contexts.game.CreateEntity();
        timer.AddTimer(_contexts.game.gameSetup.value.FlowFieldSettings.FlowFieldUpdateCooldown);
        timer.AddEntityRef(flowField.id.Value);
        timer.hasFieldUpdateCooldown = true;
        flowField.hasFieldUpdateCooldown = true;
    }

    private void IncludeMoversIntoFlowField(GameEntity flowField, HashSet<GameEntity> movers)
    {
        var fieldSettings = _contexts.game.gameSetup.value.FlowFieldSettings;

        var ffComponent = flowField.flowField;

        foreach (var e in movers)
        {
            var pos = e.position.Value;
            var (x, y) = ffComponent.GetIndex(pos);

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

                    if (ffComponent.IsIndexValid(indexX, indexY) &&
                        ffComponent.BackField[indexX][indexY] != int.MaxValue)
                    {
                        ffComponent.BackField[indexX][indexY] += fieldSettings.MoverRepulsionValue / dist;
                    }
                }
            }
        }
    }

    private void IncludeTempAvoidanceZones(GameEntity flowField)
    {
        var ffComponent = flowField.flowField;

        foreach (var e in _tempObstaclesGroup.GetEntities())
        {
            if (e.isTimerCompleted)
            {
                e.isDestroyed = true;
                continue;
            }

            var pos = e.position.Value;
            var (x, y) = ffComponent.GetIndex(pos);

            for (int i = -e.flowFieldTemporaryObstacle.Radius; i <= e.flowFieldTemporaryObstacle.Radius; i++)
            {
                for (int j = -e.flowFieldTemporaryObstacle.Radius; j <= e.flowFieldTemporaryObstacle.Radius; j++)
                {
                    float dist = Mathf.Sqrt(i * i + j * j);
                    if (dist > e.flowFieldTemporaryObstacle.Radius)
                        continue;

                    var indexX = x + i;
                    var indexY = y + j;

                    if (ffComponent.IsIndexValid(indexX, indexY) &&
                        ffComponent.BackField[indexX][indexY] != int.MaxValue)
                    {
                        ffComponent.BackField[indexX][indexY] +=
                            Mathf.RoundToInt(e.flowFieldTemporaryObstacle.InitialWeight / (dist + 1));
                    }
                }
            }
        }
    }
}