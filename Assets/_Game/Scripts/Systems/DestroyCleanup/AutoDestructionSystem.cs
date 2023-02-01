using Entitas;
using UnityEngine;

public class AutoDestructionSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _autodestructGroup;

    public AutoDestructionSystem(Contexts contexts)
    {
        _contexts = contexts;
        _autodestructGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.AutoDestruction).NoneOf(GameMatcher.Destroyed));
    }

    public void Execute()
    {
        foreach (var e in _autodestructGroup.GetEntities())
        {
            float timeLeft = e.autoDestruction.Delay - Time.deltaTime;
            if (timeLeft < 0)
            {
                e.isDestroyed = true;
            }
            else
            {
                e.ReplaceAutoDestruction(timeLeft);
            }
        }
    }
}