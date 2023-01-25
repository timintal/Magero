using System;
using Entitas;

[Flags]
public enum TargetType
{
    Invincible = 0,
    Player = 1 << 0,
    Enemy = 1 << 1,
    All = 0xFFFFFFF
}

[Game]
public class TargetComponent : IComponent
{
    public TargetType TargetType;
}