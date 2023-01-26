using Entitas;
using UnityEngine;

public class LevelStageProgressSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _spawnerGroup;
    private IGroup<GameEntity> _targetsGroup;
    private IGroup<GameEntity> _CurrentLevelStageGroup;

    public LevelStageProgressSystem(Contexts contexts)
    {
        _contexts = contexts;
        _spawnerGroup = _contexts.game.GetGroup(GameMatcher.EnemySpawner);
        _targetsGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Target, GameMatcher.Health));
        _CurrentLevelStageGroup = _contexts.game.GetGroup(GameMatcher.CurrentLevelStage);
    }

    public void Execute()
    {
        int needToKillUnits = 0;
        int unitsLeftToKill = 0;

        foreach (var spawner in _spawnerGroup.GetEntities())
        {
            needToKillUnits += spawner.enemySpawner.UnitsToSpawn;
            
            unitsLeftToKill += spawner.enemySpawner.UnitsToSpawn - spawner.enemySpawner.UnitsSpawned;
        }

        foreach (var e in _targetsGroup.GetEntities())
        {
            if (e.target.TargetType == TargetType.Enemy)
            {
                unitsLeftToKill++;
            }
        }
        
        if (unitsLeftToKill == 0)
        {
            var currLevelStage = _CurrentLevelStageGroup.GetSingleEntity();
            currLevelStage.ReplaceCurrentLevelStage(currLevelStage.currentLevelStage.Index + 1);
        }
    }
}