using Entitas;
using UnityEngine;

[Game]
public class BlackHoleShooterComponent : IComponent
{
    public float ExplosionRadius;
    public float BlackHolePullSpeed;
    public float BlackHolePullRadius;
    public float BlackHoleLifetime;
    public GameObject BlackHolePrefab;
    public GameObject ExplosionPrefab;
    
}