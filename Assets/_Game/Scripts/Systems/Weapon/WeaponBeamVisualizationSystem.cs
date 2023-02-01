using Entitas;

public class WeaponBeamVisualizationSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _laserGroup;

    public WeaponBeamVisualizationSystem(Contexts contexts)
    {
        _contexts = contexts;
        _laserGroup = _contexts.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.BeamRenderer, 
            GameMatcher.Transform, 
            GameMatcher.WeaponHitPoint));
        
    }

    public void Execute()
    {
        foreach (var e in _laserGroup.GetEntities())
        {
            if (e.isWeaponDisabled)
            {
                e.beamRenderer.Renderer.enabled = false;
            }
            else
            {
                e.beamRenderer.Renderer.enabled = true;
                e.beamRenderer.Renderer.SetPosition(0, e.transform.Transform.position);
                e.beamRenderer.Renderer.SetPosition(1, e.weaponHitPoint.Value);
            }
        }
    }
}