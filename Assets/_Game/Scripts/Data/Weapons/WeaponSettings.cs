using System.Collections.Generic;
using _Game.Data;
using Game.Config.Model;
using UnityEngine;

public abstract class WeaponSettings : ScriptableObject
{
    public Sprite WeaponSprite;
    public string WeaponName;
    public string WeaponKey;

    public abstract WeaponType Type { get; }
    public abstract void ConfigWeaponEntity(GameEntity entity, GameSceneReferences sceneReferences, int armIndex, WeaponData weaponData, GameConfig gameConfig);

    public abstract List<UpgradableWeaponParam> UpgradableParams { get; }
}