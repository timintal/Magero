using Entitas;
using UnityEngine;

[Game]
public class LaserShooterComponent : IComponent
{
    public LineRenderer Renderer;
    public float DamagePerSecond;
}