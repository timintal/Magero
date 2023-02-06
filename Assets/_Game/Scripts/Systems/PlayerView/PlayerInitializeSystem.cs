using _Game.Data;
using Entitas;
using UnityEngine;

public class PlayerInitializeSystem : IInitializeSystem
{
    Contexts _contexts;
    private readonly PlayerData _playerData;

    public PlayerInitializeSystem(Contexts contexts, PlayerData playerData)
    {
        _contexts = contexts;
        _playerData = playerData;
    }

    public void Initialize()
    {
        var gameSetup = _contexts.game.gameSetup.value;
        var sceneReferences = _contexts.game.gameSceneReferences.value;

        var playerEntity = _contexts.game.CreateEntity();
        playerEntity.AddHealth(gameSetup.PlayerSettings.Health);
        playerEntity.AddMaxHealth(gameSetup.PlayerSettings.Health);
        playerEntity.AddTarget(TargetType.Player);
        playerEntity.AddTransform(sceneReferences.CameraTransform);
        playerEntity.AddHealthBarUI(sceneReferences.PlayerHealthBar);
        playerEntity.isPlayer = true;

        int armIndex = 0;

        if (_playerData.LeftHandWeapon != WeaponType.None)
        {
            var leftWeapon = gameSetup.GetWeaponSettings(_playerData.LeftHandWeapon);
            var weaponEntity = _contexts.game.CreateEntity();
            leftWeapon.ConfigWeaponEntity(weaponEntity, sceneReferences, armIndex++);
        }
        
        if (_playerData.RightHandWeapon != WeaponType.None)
        {
            var rightWeapon = gameSetup.GetWeaponSettings(_playerData.RightHandWeapon);
            var weaponEntity = _contexts.game.CreateEntity();
            rightWeapon.ConfigWeaponEntity(weaponEntity, sceneReferences, armIndex++);
        }
        
        

        for (int i = armIndex; i < sceneReferences.Arms.Length; i++)
        {
            sceneReferences.Arms[i].gameObject.SetActive(false);
        }
    }
}