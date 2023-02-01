using Entitas;
using UnityEngine;

[Game]
public class GasProjectileComponent : IComponent
{
    public float CloudRadius;
    public float MoveSpeedMultiplier;
    public GameObject CloudPrefab;
}