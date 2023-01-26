using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class RagdollCreationSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;

    public RagdollCreationSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Destroyed.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isDestroyed && 
               entity.isRagdollDeath &&
               entity.hasTransform &&
               entity.hasDamageSourcePosition &&
               entity.hasPosition;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.isDestroyed = false;
            e.isFlowFieldMover = false;
            
            if (e.hasDirection)
                e.RemoveDirection();
            
            if (e.hasSpeed)
                e.RemoveSpeed();
            
            if (e.hasTarget)
                e.RemoveTarget();
            
            if (e.hasHealth)
                e.RemoveHealth();

            var diff = e.position.Value - e.damageSourcePosition.Value;
            diff.y = 0;
            diff.Normalize();
            diff *= Random.Range(0.5f, 1.5f);
            diff.y = 3;
            e.AddRagdollCurrentVelocity(diff * 5);
            e.AddRagdollAngularVelocity(Random.insideUnitSphere, Random.Range(0f, 360f));
            e.AddRagdollRemoveTimer(3);
            
            if (e.hasRenderer)
            {
                MaterialPropertyBlock block = new();
                e.renderer.Renderer.GetPropertyBlock(block);
                block.SetColor("_BaseColor", Color.grey);
                e.renderer.Renderer.SetPropertyBlock(block);
            }
        }
    }
}