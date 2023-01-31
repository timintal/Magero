using Entitas;
using UnityEngine;

public class StunMovementSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _stunnedGroup;

    public StunMovementSystem(Contexts contexts)
    {
        _contexts = contexts;
        _stunnedGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Stunned, GameMatcher.Speed));
    }

    public void Execute()
    {
        foreach (var e in _stunnedGroup.GetEntities())
        {
            var stunnedTimeLeft = e.stunned.TimeLeft - Time.deltaTime;
            if (stunnedTimeLeft > 0)
            {
                e.ReplaceStunned(stunnedTimeLeft);
            }
            else
            {
                e.RemoveStunned();
            }
            e.ReplaceSpeed(0, e.speed.BaseValue);
        }
    }
}