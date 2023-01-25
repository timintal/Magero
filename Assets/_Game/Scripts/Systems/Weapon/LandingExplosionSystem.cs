using Entitas;

public class LandingExplosionSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _projectilesGroup;

    public LandingExplosionSystem(Contexts contexts)
    {
        _contexts = contexts;
        _projectilesGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Projectile, GameMatcher.ExplodableProjectile, GameMatcher.Position));
    }

    public void Execute()
    {
        foreach (var e in _projectilesGroup.GetEntities())
        {
            var positionValue = e.position.Value;
            if (positionValue.y < 0)
            {
                e.isDestroyed = true;
                var explosion = _contexts.game.CreateEntity();
                positionValue.y = 0;
                explosion.AddPosition(positionValue);
                explosion.AddExplosion(e.explodableProjectile.ExplosionRadius);
                explosion.AddDamage(e.damage.Damage);
            }
        }
    }
}