public class ShootingFeature : Feature
{
    public ShootingFeature(Contexts contexts)
    {
        Add(new WeaponCooldownSystem(contexts));
        Add(new ExplodableProjectileShootingSystem(contexts));
        Add(new LaserShootingSystem(contexts));
    }    
}