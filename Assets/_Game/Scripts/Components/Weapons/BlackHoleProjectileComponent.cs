using Entitas;
using UnityEngine;

[Game]
public class BlackHoleProjectileComponent : IComponent
{
    public float ExplosionRadius;
    public float PullSpeed;
    public float PullRadius;
    public GameObject BlackHolePrefab;
    public float Lifetime;
}