using Entitas;
using UnityEngine;

[Game]
public class ExplodableProjectileShooterComponent : IComponent
{
    public float ExplosionRadius;
    public GameObject ExplosionPrefab;
}