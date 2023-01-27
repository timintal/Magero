using Entitas;
using Sirenix.OdinInspector;
using UnityEngine;

[Game]
public class EnemySpawnerComponent : IComponent
{
    public Vector2 SpawnDelayRange;
    public Vector2Int SpawnCountRange;
    public Vector2 SpawnArea;

    public int UnitsToSpawn;

    public int UnitsSpawned;
    public float TimeToNextSpawn;

    public EnemySettings EnemyToSpawn;
}