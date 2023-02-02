using Entitas;
using UnityEngine;

public class ForcedMovementSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _movementsGroup;

    public ForcedMovementSystem(Contexts contexts)
    {
        _contexts = contexts;
        _movementsGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.ForcedMovement, GameMatcher.Direction, GameMatcher.EntityRef));
    }

    public void Execute()
    {
        foreach (var e in _movementsGroup.GetEntities())
        {
            var movable = _contexts.game.GetEntityWithId(e.entityRef.EntityId);
            if (movable != null && movable.hasPosition)
            {
               var newPos =  movable.position.Value + e.direction.Value * e.forcedMovement.Speed * Time.deltaTime;

               bool isPositionPassable = true;
               if (movable.hasFlowFieldMover)
               {
                   var flowField = _contexts.game.GetEntityWithId(movable.flowFieldMover.FlowFieldIndex).flowField;
                   isPositionPassable = flowField.IsPassablePosition(newPos, int.MaxValue - 100);
               }

               if (isPositionPassable)
               {
                   movable.ReplacePosition(newPos);
               }
               
            }

            e.isDestroyed = true;
        }
    }
}