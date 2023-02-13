using Entitas;
using UnityEngine;

public class OutOfScreenEnemiesSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private IGroup<GameEntity> _enemiesGroup;
    private Camera _gameCamera;

    public OutOfScreenEnemiesSystem(Contexts contexts)
    {
        _contexts = contexts;
        _enemiesGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Position, GameMatcher.Target));
        _gameCamera = _contexts.game.gameSceneReferences.value.CameraTransform.GetComponent<Camera>();
    }
    
    public void Execute()
    {
        foreach (var e in _enemiesGroup.GetEntities())
        {
            if (e.target.TargetType == TargetType.Enemy)
            {
                var viewportPoint = _gameCamera.WorldToViewportPoint(e.position.Value);
                if (viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1)
                {
                    e.AddOutOfScreen(viewportPoint);
                }
            }
        }
    }
}