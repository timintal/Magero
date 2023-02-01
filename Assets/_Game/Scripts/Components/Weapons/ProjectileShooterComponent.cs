using Entitas;
using UnityEngine;

[Game]
public class ProjectileShooterComponent : IComponent
{
    public float Cooldown;
    public GameObject Prefab;
    public float ProjectileSpeed;
    public TargetType Target;
}