using Entitas;
using UnityEngine;

public class RagdollUpdateSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _ragdollsGroup;

    public RagdollUpdateSystem(Contexts contexts)
    {
        _contexts = contexts;
        _ragdollsGroup = contexts.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.RagdollDeath, 
            GameMatcher.RagdollAngularVelocity, 
            GameMatcher.RagdollCurrentVelocity,
            GameMatcher.Position));
    }

    public void Execute()
    {
        foreach (var e in _ragdollsGroup.GetEntities())
        {
            var timeLeft = e.ragdollRemoveTimer.TimeLeft - Time.deltaTime;
            if (timeLeft < 0)
            {
                e.isDestroyed = true;
            }
            else
            {
                e.ReplaceRagdollRemoveTimer(timeLeft);

                var position = e.position.Value;
                var currVelocity = e.ragdollCurrentVelocity.Value;
                currVelocity += Vector3.down * 20 * Time.deltaTime;
                position += currVelocity * Time.deltaTime;
                
                e.transform.Transform.Rotate(e.ragdollAngularVelocity.Axis, e.ragdollAngularVelocity.AnglePerSecond * Time.deltaTime);
                
                e.ReplaceRagdollCurrentVelocity(currVelocity);
                e.ReplacePosition(position);
            }
        }
    }
}