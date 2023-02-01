using Entitas;
using UnityEngine;

public class ForcedMovementSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _movementsGroup;
    private IGroup<GameEntity> _flowFieldGroup;

    public ForcedMovementSystem(Contexts contexts)
    {
        _contexts = contexts;
        _movementsGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.ForcedMovement, GameMatcher.Direction, GameMatcher.EntityRef));
        _flowFieldGroup = contexts.game.GetGroup(GameMatcher.FlowField);
    }

    public void Execute()
    {
        var flowField = _flowFieldGroup.GetSingleEntity().flowField;

        foreach (var e in _movementsGroup.GetEntities())
        {
            var movable = _contexts.game.GetEntityWithId(e.entityRef.EntityId);
            if (movable != null && movable.hasPosition)
            {
               var newPos =  movable.position.Value + e.direction.Value * e.forcedMovement.Speed * Time.deltaTime;

               if (flowField.IsPassablePosition(newPos, int.MaxValue - 100))
               {
                   movable.ReplacePosition(newPos);
               }
            }

            e.isDestroyed = true;
        }
    }
}