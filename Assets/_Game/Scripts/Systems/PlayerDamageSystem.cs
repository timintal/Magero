using Entitas;
using UnityEngine;

public class PlayerDamageSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _actorsGroup;
    private IGroup<GameEntity> _playerGroup;
    private PlayerSettings _playerSettings;

    public PlayerDamageSystem(Contexts contexts)
    {
        _contexts = contexts;
        _actorsGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Target, GameMatcher.Position).NoneOf(GameMatcher.Player));
        _playerGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.Health, GameMatcher.Transform));
        _playerSettings = contexts.game.gameSetup.value.PlayerSettings;
    }

    public void Execute()
    {
        foreach (var playerEntity in _playerGroup.GetEntities())
        {
            int totalDamage = 0;
            
            var playerPosition = playerEntity.transform.Transform.position;
            playerPosition.y = 0;
            
            foreach (var e in _actorsGroup.GetEntities())
            {
                if (e.target.TargetType == TargetType.Enemy)
                {
                    var enemyPosition = e.position.Value;
                    enemyPosition.y = 0;
                    if ((playerPosition - enemyPosition).sqrMagnitude <
                        _playerSettings.DistanceForDamage * _playerSettings.DistanceForDamage)
                    {
                        totalDamage += 1;
                        e.isDestroyed = true;
                    }
                }
            }

            var healthLeft = playerEntity.health.Value - totalDamage;
            playerEntity.ReplaceHealth(Mathf.Max(0, healthLeft));
        }
        
    }
}