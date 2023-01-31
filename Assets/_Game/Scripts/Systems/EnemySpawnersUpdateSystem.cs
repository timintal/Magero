using System;
using Entitas;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnersUpdateSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _enemySpawnersGroup;

    public EnemySpawnersUpdateSystem(Contexts contexts)
    {
        _contexts = contexts;
        _enemySpawnersGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.EnemySpawner, GameMatcher.Position));
    }

    public void Execute()
    {
        foreach (var e in _enemySpawnersGroup.GetEntities())
        {
            var timeToNextSpawn = e.enemySpawner.TimeToNextSpawn - Time.deltaTime;
            if (timeToNextSpawn < 0)
            {
                var spawnRequestEntity = _contexts.game.CreateEntity();
                var unitsCount = Random.Range(e.enemySpawner.SpawnCountRange.x, e.enemySpawner.SpawnCountRange.y);
                spawnRequestEntity.AddEnemySpawnRequest(e.enemySpawner.EnemyToSpawn, e.enemySpawner.SpawnArea, unitsCount);
                spawnRequestEntity.AddPosition(e.position.Value);

                e.ReplaceEnemySpawner(e.enemySpawner.SpawnDelayRange,
                    e.enemySpawner.SpawnCountRange,
                    e.enemySpawner.SpawnArea,
                    e.enemySpawner.UnitsToSpawn,
                    e.enemySpawner.UnitsSpawned + unitsCount,
                    Random.Range(e.enemySpawner.SpawnDelayRange.x, e.enemySpawner.SpawnDelayRange.y),
                    e.enemySpawner.EnemyToSpawn);
                
                if (e.enemySpawner.UnitsToSpawn <= e.enemySpawner.UnitsSpawned)
                {
                    e.isDestroyed = true;
                }
            }
            else
            {
                e.ReplaceEnemySpawner(e.enemySpawner.SpawnDelayRange,
                    e.enemySpawner.SpawnCountRange,
                    e.enemySpawner.SpawnArea,
                    e.enemySpawner.UnitsToSpawn,
                    e.enemySpawner.UnitsSpawned,
                    timeToNextSpawn,
                    e.enemySpawner.EnemyToSpawn);
            }
        }
    }
}