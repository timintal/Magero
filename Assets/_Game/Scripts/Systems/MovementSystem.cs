using Entitas;
using UnityEngine;

public class MovementSystem : IExecuteSystem
{
    private Contexts _contexts;
    private IGroup<GameEntity> _movableEntities;
    public MovementSystem(Contexts context)
    {
        _contexts = context;
        _movableEntities = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Position, GameMatcher.Direction, GameMatcher.Speed));
    }
    
    public void Execute()
    {
        foreach (var entity in _movableEntities.GetEntities())
        {
            var positionValue = entity.position.Value;
            entity.ReplacePosition(positionValue + entity.direction.Value * entity.speed.Value * Time.deltaTime);
        }
    }
}