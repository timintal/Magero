using Entitas;
using UnityEngine;

public class WindImpulseUpdateSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _windImpulseGroup;

    public WindImpulseUpdateSystem(Contexts contexts)
    {
        _contexts = contexts;
        _windImpulseGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.WindImpulse, GameMatcher.Damping, GameMatcher.Position));
    }

    public void Execute()
    {
        foreach (var e in _windImpulseGroup.GetEntities())
        {
            var currentPower = e.windImpulse.Power - e.damping.DecreasePerSecond * Time.deltaTime;

            if (currentPower < 0)
            {
                e.RemoveWindImpulse();
            }
            else
            {
                var forcedMovementEntity = _contexts.game.CreateEntity();
                forcedMovementEntity.AddForcedMovement(currentPower);
                forcedMovementEntity.AddDirection(e.windImpulse.Direction);
                forcedMovementEntity.AddEntityRef(e.id.Value);
                
                e.ReplaceWindImpulse(e.windImpulse.Direction, currentPower);
            }
        }
    }
}