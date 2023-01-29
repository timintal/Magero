using Entitas;

public class LaserVisualizationSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _laserGroup;

    public LaserVisualizationSystem(Contexts contexts)
    {
        _contexts = contexts;
        _laserGroup = _contexts.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.LaserShooter, 
            GameMatcher.Transform, 
            GameMatcher.Direction,
            GameMatcher.LaserHitPoint, 
            GameMatcher.LaserSparkles));
        
    }

    public void Execute()
    {
        foreach (var e in _laserGroup.GetEntities())
        {
            if (e.isWeaponDisabled)
            {
                e.laserShooter.Renderer.enabled = false;
                e.laserSparkles.SparklesTransform.position = e.transform.Transform.position;
            }
            else
            {
                e.laserShooter.Renderer.enabled = true;
                e.laserShooter.Renderer.SetPosition(0, e.transform.Transform.position);
                e.laserShooter.Renderer.SetPosition(1, e.laserHitPoint.Value);
                e.laserSparkles.SparklesTransform.position = e.laserHitPoint.Value;
            }
        }
    }
}