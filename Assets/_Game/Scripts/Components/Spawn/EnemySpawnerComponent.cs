using Entitas;
using UnityEngine;

[Game]
public class EnemySpawnerComponent : IComponent
{
    public Vector2 SpawnDelayRange;
    public Vector2Int SpawnCountRange;

    public int UnitsToSpawn;

    public int UnitsSpawned;
    public float TimeToNextSpawn;
}