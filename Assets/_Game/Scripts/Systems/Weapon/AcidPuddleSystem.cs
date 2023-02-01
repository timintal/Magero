using Entitas;
using UnityEngine;

public class AcidPuddleSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _acidPuddleGroup;

    
    public AcidPuddleSystem(Contexts contexts)
    {
        _contexts = contexts;
        _acidPuddleGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.AcidPuddle, GameMatcher.Transform, GameMatcher.Radius, GameMatcher.AutoDestruction));
    }

    public void Execute()
    {
        foreach (var e in _acidPuddleGroup.GetEntities())
        {
            var newRadius = e.acidPuddle.InitialRadius *
                            e.acidPuddle.RadiusCurve.Evaluate(1 - e.autoDestruction.Delay /
                                e.acidPuddle.PuddleLifetime);
            
            e.ReplaceRadius(Mathf.Lerp(e.radius.Value, newRadius, 10f * Time.deltaTime));

            e.transform.Transform.localScale = Vector3.one * newRadius;
        }
    }
}