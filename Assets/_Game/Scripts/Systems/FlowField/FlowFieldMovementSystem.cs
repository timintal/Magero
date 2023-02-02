using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class FlowFieldMovementSystem : IExecuteSystem
{
    private readonly Contexts _contexts;

    private IGroup<GameEntity> _moversWithDelay;
    private IGroup<GameEntity> _flowFieldsGroup;

    private List<GameEntity> _removeCache = new(2048);
    
    public FlowFieldMovementSystem(Contexts contexts)
    {
        _contexts = contexts;

        _moversWithDelay = contexts.game.GetGroup(GameMatcher.FlowFieldDirectionUpdateDelay);

        _flowFieldsGroup = contexts.game.GetGroup(GameMatcher.FlowField);
    }

    public void Execute()
    {
        if (_flowFieldsGroup.count == 0) return;
        
        var fieldSettings = _contexts.game.gameSetup.value.FlowFieldSettings;
        var maxCalculationDistance = fieldSettings.MaxCalculationDistance;

        foreach (var e in _flowFieldsGroup)
        {
            var movers = _contexts.game.GetEntitiesWithFlowFieldMover(e.id.Value);
            var flowField = e.flowField;
            
            foreach (var mover in movers)
            {
                if (mover.hasFlowFieldDirectionUpdateDelay) continue;
                
                var direction = flowField.GetDirection(mover.position.Value, maxCalculationDistance, out DirectionFetchResult result,
                    out int targetX, out int targetY);
                
                direction.y = 0;
                
                var hasDirection = mover.hasDirection;
                if (result != DirectionFetchResult.NotFound)
                {
                    if (hasDirection && result != DirectionFetchResult.FoundForced)
                    {
                        direction = Vector3.RotateTowards(mover.direction.Value, direction, 10f * Time.deltaTime, 0);
                    }

                    flowField.CurrentField[targetX][targetY] += fieldSettings.StepWeight / 4;
                    mover.ReplaceDirection(direction);
                    mover.AddFlowFieldDirectionUpdateDelay(Random.Range(0.04f, 0.15f));
                }
                else
                {
                    if (hasDirection)
                    {
                        mover.RemoveDirection();
                    }
                }
            }
        }
        
        _removeCache.Clear();
        foreach (var e in _moversWithDelay)
        {
            var newDelay = e.flowFieldDirectionUpdateDelay.Delay - Time.deltaTime;
            if (newDelay > 0)
            {
                e.ReplaceFlowFieldDirectionUpdateDelay(newDelay);
            }
            else
            {
                _removeCache.Add(e);
            }
        }

        foreach (var e in _removeCache)
        {
            e.RemoveFlowFieldDirectionUpdateDelay();
        }
    }
}