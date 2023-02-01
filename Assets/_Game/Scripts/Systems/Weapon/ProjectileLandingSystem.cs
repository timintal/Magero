using Entitas;

public class ProjectileLandingSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _projectilesGroup;

    public ProjectileLandingSystem(Contexts contexts)
    {
        _contexts = contexts;
        _projectilesGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Projectile,  GameMatcher.Position).NoneOf(GameMatcher.ProjectileLanded));
    }

    public void Execute()
    {
        foreach (var e in _projectilesGroup.GetEntities())
        {
            var positionValue = e.position.Value;
            if (positionValue.y < 0)
            {
                e.isProjectileLanded = true;
                
                positionValue.y = 0;
                e.ReplacePosition(positionValue);
            }
        }
    }
}