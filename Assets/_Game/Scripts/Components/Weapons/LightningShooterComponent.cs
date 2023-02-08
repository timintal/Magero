using Entitas;

[Game]
public class LightningShooterComponent : IComponent
{
    public float Cooldown;
    public float EffectRadius;
    public float TargetDamage;
    public float AOEDamage;
    public float StunDuration;
}