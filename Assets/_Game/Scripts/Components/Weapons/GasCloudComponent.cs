using Entitas;

[Game]
public class GasCloudComponent : IComponent
{
    public float CloudRadius;
    public float DamagePerSecon;
    public float MoveSpeedMultiplier;
    public TargetType Targets;
}