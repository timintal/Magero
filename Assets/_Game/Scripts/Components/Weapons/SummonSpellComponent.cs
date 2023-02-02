using Entitas;
using UnityEngine;

[Game]
public class SummonSpellComponent : IComponent
{
    public float Cooldown;
    public int UnitsCount;
    public float UnitsSpeed;
    public float UnitRadius;
    public float UnitLifetime;
    public GameObject UnitExplosionPrefab;
}