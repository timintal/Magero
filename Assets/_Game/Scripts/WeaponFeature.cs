public class WeaponFeature : Feature
{
    public WeaponFeature(Contexts contexts)
    {
        Add(new WeaponCooldownSystem(contexts));
        
        Add(new ProjectileLandingSystem(contexts));
        Add(new ProjectileExplodeSystem(contexts));
        
        Add(new ExplodableProjectileShootingSystem(contexts));
        
        Add(new LaserShootingSystem(contexts));
        
        Add(new LightningStrikeShootingSystem(contexts));
        
        Add(new AcidStreamSystem(contexts));
        Add(new AcidPuddleSystem(contexts));
        
        Add(new DamageOverTimeZoneSystem(contexts));
        
        Add(new GasProjectileShootingSystem(contexts));
        Add(new GasProjectileImpactSystem(contexts));
    }
}