using Entitas;
using UnityEngine;

public class PlayerWeaponDirectionUpdateSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _playerWeaponGroup;
    private readonly IGroup<GameEntity> _cameraGroup;

    public PlayerWeaponDirectionUpdateSystem(Contexts contexts)
    {
        _contexts = contexts;
        _playerWeaponGroup = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.ProjectileShooter, GameMatcher.Player, GameMatcher.Transform));
        _cameraGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Camera, GameMatcher.Transform));

    }

    public void Execute()
    {
        var cameraEntity = _cameraGroup.GetSingleEntity();

        var cameraTransform = cameraEntity.transform.Transform;
        var cameraForward = cameraTransform.forward;

        Vector3 direction = cameraForward;
        var playerWeaponEntity = _playerWeaponGroup.GetSingleEntity();

        if (cameraForward.y < 0)
        {
            var factor = cameraTransform.position.y / cameraForward.y;
            Vector3 impactPoint = cameraTransform.position - cameraForward * factor;

            direction = impactPoint - playerWeaponEntity.transform.Transform.position;
            direction.Normalize();
        }
        
        playerWeaponEntity.ReplaceDirection(direction);
        
    }
}