using Entitas;

public class AnimatorSpeedSyncSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _animatorSyncGroup;

    public AnimatorSpeedSyncSystem(Contexts contexts)
    {
        _contexts = contexts;
        _animatorSyncGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.AnimatorSpeedSync, GameMatcher.Animator, GameMatcher.Speed));
    }

    public void Execute()
    {
        foreach (var e in _animatorSyncGroup.GetEntities())
        {
            e.animator.Value.SetFloat(e.animatorSpeedSync.PropertyHash, e.speed.Value / e.speed.BaseValue);
        }
    }
}