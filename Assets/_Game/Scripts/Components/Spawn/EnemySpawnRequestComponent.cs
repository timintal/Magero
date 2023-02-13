using Entitas;
using UnityEngine;

[Game]
public class EnemySpawnRequestComponent : IComponent
{
    public EnemySettings EnemySettings;
    public Vector2 Bounds;
    public int Count;
    public int EnemyLevel;
}