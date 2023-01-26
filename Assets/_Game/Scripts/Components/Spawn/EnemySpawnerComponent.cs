using Entitas;
using Sirenix.OdinInspector;
using UnityEngine;

[Game]
public class EnemySpawnerComponent : IComponent
{
    public Vector2 SpawnDelayRange;
    public Vector2Int SpawnCountRange;

    public int UnitsToSpawn;

    // [ReadOnly]
    public int UnitsSpawned;
    // [ReadOnly] 
    public float TimeToNextSpawn;
}