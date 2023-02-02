using Entitas;
using UnityEngine;


public class WeaponHitPointFxUpdateSystem : IExecuteSystem
{
    Contexts _contexts;
    private IGroup<GameEntity> _laserGroup;

    public WeaponHitPointFxUpdateSystem(Contexts contexts)
    {
        _contexts = contexts;
        _laserGroup = _contexts.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.HitPointEffect, 
            GameMatcher.Transform, 
            GameMatcher.WeaponHitPoint));
        
    }

    public void Execute()
    {
        foreach (var e in _laserGroup.GetEntities())
        {
            if (e.isWeaponDisabled)
            {
                e.hitPointEffect.FxTransform.position = new Vector3(-100,-100,-100);
            }
            else
            {
                e.hitPointEffect.FxTransform.position = e.weaponHitPoint.Value;
            }
        }
    }
}