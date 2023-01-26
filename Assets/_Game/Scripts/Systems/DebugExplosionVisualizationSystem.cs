using Entitas;
using UnityEngine;

public class DebugExplosionVisualizationSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _explosionVisualGroup;

    public DebugExplosionVisualizationSystem(Contexts contexts)
    {
        _contexts = contexts;
        _explosionVisualGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.ExplosionVisualization, GameMatcher.Transform, GameMatcher.Radius));
    }

    public void Execute()
    {
        foreach (var e in _explosionVisualGroup.GetEntities())
        {
            var explosionVisualizationTimeLeft = e.explosionVisualization.TimeLeft - Time.deltaTime;
            if (explosionVisualizationTimeLeft < 0)
            {
                e.isDestroyed = true;
            }
            else
            {
                e.ReplaceExplosionVisualization(explosionVisualizationTimeLeft);
                e.transform.Transform.localScale = Vector3.one * e.radius.Value * 2f;
            }
        }
    }
}