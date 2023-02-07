using _Game.Data;
using UnityEngine;

public abstract class WeaponSettings : ScriptableObject
{
    public Sprite WeaponSprite;
    public string WeaponName;
    
    public abstract WeaponType Type { get; }
    public abstract void ConfigWeaponEntity(GameEntity entity, GameSceneReferences sceneReferences, int armIndex);

}
