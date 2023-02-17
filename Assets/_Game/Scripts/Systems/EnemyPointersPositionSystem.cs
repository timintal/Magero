using Entitas;
using UIFramework.Runtime;
using UnityEngine;

public class EnemyPointersPositionSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private EnemyPointersPresenter _enemyPointersPresenter;
    private IGroup<GameEntity> _outOfScreenGroup;
    private float _screenAcpectRation;

    public EnemyPointersPositionSystem(Contexts contexts, UIFrame uiFrame)
    {
        _contexts = contexts;
        _enemyPointersPresenter = uiFrame.GetScreen<GamePlayScreen>().EnemyPointersPresenter;
        _outOfScreenGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.OutOfScreen));
        _screenAcpectRation = (float)Screen.width / Screen.height;
    }

    public void Execute()
    {
        _enemyPointersPresenter.ResetPointers();
        foreach (var e in _outOfScreenGroup.GetEntities())
        {
            var viewportPosition = e.outOfScreen.ViewportPosition;
            if (viewportPosition.z < 0)
            {
                viewportPosition.x = 1 - viewportPosition.x;
                viewportPosition.y = 1 - viewportPosition.y;
                viewportPosition.z = 0;
            }

            viewportPosition.x -= 0.5f;
            viewportPosition.y -= 0.5f;
            
            float preferredScale = Mathf.Clamp(Mathf.Max(Mathf.Abs(viewportPosition.x), Mathf.Abs(viewportPosition.y)), 1, 2);
            
            if (Mathf.Abs(viewportPosition.x ) > Mathf.Abs( viewportPosition.y * _screenAcpectRation))
            {
                viewportPosition.y /= Mathf.Abs(viewportPosition.x);
                viewportPosition.x /= Mathf.Abs(viewportPosition.x);
            }
            else
            {
                viewportPosition.x /= Mathf.Abs(viewportPosition.y);
                viewportPosition.y /= Mathf.Abs(viewportPosition.y);
            }
            
            
            _enemyPointersPresenter.AddPointer(Vector3.one * 0.5f + viewportPosition * 0.5f, preferredScale);
        }

        _enemyPointersPresenter.HideUnusedPointers();
    }

    Vector3 Vector3MaxOffset(Vector3 vector)
    {
        Vector3 returnVector = vector;
        float max = Mathf.Max(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
        if (max > 0)
        {
            returnVector.x = vector.x / max;
            returnVector.y = vector.y / max;
        }

        return returnVector;
    }
}