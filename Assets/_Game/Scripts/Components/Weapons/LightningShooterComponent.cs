using Entitas;

[Game]
public class LightningShooterComponent : IComponent
{
    public float Cooldown;
    public float EffectRadius;
    public int TargetDamage;
    public int AOEDamage;
    public float StunDuration;
}