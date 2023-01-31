using Entitas;
using UnityEngine;

[Game]
public class AcidStreamComponent : IComponent
{
    public float Cooldown;
    public float PoolRadius;
    public GameObject PoolPrefab;
    public float DamagePerSecond;
    public float RadiusDecreasePerSecond;
}