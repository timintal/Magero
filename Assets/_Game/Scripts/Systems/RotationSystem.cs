using Entitas;
using UnityEngine;

public class RotationSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _movableEntities;


    public RotationSystem(Contexts contexts)
    {
        _contexts = contexts;
        _movableEntities = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Rotation, GameMatcher.Direction));
    }

    public void Execute()
    {
        foreach (var entity in _movableEntities.GetEntities())
        {
            var rotation = entity.rotation.Value;
            
            entity.ReplaceRotation(Quaternion.Lerp(rotation, Quaternion.LookRotation(entity.direction.Value, Vector3.up), 5 * Time.deltaTime));
        }
    }
}