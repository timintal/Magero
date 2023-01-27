using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class FlowFieldMovementSystem : IExecuteSystem
{
    private readonly Contexts _contexts;

    private IGroup<GameEntity> _moversGroup;
    private IGroup<GameEntity> _moversWithDelay;
    private IGroup<GameEntity> _flowFieldsGroup;

    private List<GameEntity> _removeCache = new(2048);
    
    public FlowFieldMovementSystem(Contexts contexts)
    {
        _contexts = contexts;

        _moversGroup = contexts.game.GetGroup(
            GameMatcher
                .AllOf(
                GameMatcher.Position,
                GameMatcher.Speed,
                GameMatcher.FlowFieldMover)
                .NoneOf(
                    GameMatcher.FlowFieldDirectionUpdateDelay
                    ));

        _moversWithDelay = contexts.game.GetGroup(GameMatcher.FlowFieldDirectionUpdateDelay);

        _flowFieldsGroup = contexts.game.GetGroup(GameMatcher.FlowField);
    }

    public void Execute()
    {
        if (_flowFieldsGroup.count == 0) return;
        
        var flowField = _flowFieldsGroup.GetSingleEntity().flowField;

        var maxCalculationDistance = _contexts.game.gameSetup.value.FlowFieldSettings.MaxCalculationDistance;
        foreach (var e in _moversGroup.GetEntities())
        {
            var direction = flowField.GetDirection(e.position.Value, maxCalculationDistance, out bool directionFound);
            
            var hasDirection = e.hasDirection;
            if (directionFound)
            {
                if (hasDirection)
                {
                    direction = Vector3.RotateTowards(e.direction.Value, direction, 15f * Time.deltaTime, 0);
                }
                e.ReplaceDirection(direction);
                e.AddFlowFieldDirectionUpdateDelay(Random.Range(0.3f, 0.5f));
            }
            else
            {
                if (hasDirection)
                {
                    e.RemoveDirection();
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