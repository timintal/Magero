using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Flags]
public enum TargetType
{
    Invincible = 0,
    Player = 1 << 0,
    Enemy = 1 << 1,
    Neutral = 1 << 2,
    All = 0xFFFFFFF
}

[Game]
public class TargetComponent : IComponent
{
    [EntityIndex]
    public TargetType TargetType;
}