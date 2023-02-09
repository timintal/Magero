using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class CreateUIOverlaySystem : ReactiveSystem<GameEntity>
{
    private readonly PoolService _poolService;
    private readonly Contexts _contexts;


    public CreateUIOverlaySystem(Contexts contexts, PoolService poolService) : base(contexts.game)
    {
        _contexts = contexts;
        _poolService = poolService;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.UIOverlay);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isUIOverlay && entity.hasTransform && entity.hasPosition && entity.hasDamage && entity.hasMaxHealth;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            var objTransform = entity.transform.Transform;
            
            objTransform.position = entity.position.Value + Vector3.up * 1.5f;
            var diff = entity.position.Value - _contexts.game.gameSceneReferences.value.CameraTransform.position;
            objTransform.localScale = diff.magnitude * 0.05f * Mathf.Lerp(1f,1.3f, entity.damage.Value / entity.maxHealth.Value) * Vector3.one;
            objTransform.rotation = Quaternion.LookRotation(diff, Vector3.up);

            objTransform.gameObject.GetComponent<TextUIOverlay>().ShowOverlayForValue(entity.damage.Value);
        }
        
    }
}