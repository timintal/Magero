using Entitas;
using UnityEngine;

[Game]
public class GasProjectileShooterComponent : IComponent
{
    public float CloudRadius;
    public float MoveSpeedMultiplier;
    public GameObject CloudPrefab;
}