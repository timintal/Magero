using Entitas;
using Entitas.CodeGeneration.Attributes;

[UI]
public class DamageForUIComponent : IComponent
{
    [PrimaryEntityIndex]
    public int TargetId;
    public float Value;
    public float Cooldown;
    public float AccumulatedDamage;
}