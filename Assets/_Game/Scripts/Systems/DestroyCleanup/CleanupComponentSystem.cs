using System;
using System.Collections.Generic;
using Entitas;

public class CleanupComponentSystem<T> : ReactiveSystem<GameEntity> where T : IComponent
{
    public CleanupComponentSystem(IContext<GameEntity> context) : base(context)
    {
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(
            new TriggerOnEvent<GameEntity>((Matcher<GameEntity>)Matcher<GameEntity>.AllOf(Array.IndexOf(GameComponentsLookup.componentTypes, typeof(T))),
                GroupEvent.Added));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.HasComponent(Array.IndexOf(GameComponentsLookup.componentTypes, typeof(T)));
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {   
            entity.RemoveComponent(Array.IndexOf(GameComponentsLookup.componentTypes, typeof(T)));
        }
    }
}