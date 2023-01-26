using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using Entitas.Unity;
using Sirenix.OdinInspector;
using UnityEngine;

public class MonoEntityLink : SerializedMonoBehaviour
{
    public bool CreateOnStart = true;
    [SerializeField, InlineEditor()] private EntityTemplate _template;
    [SerializeField] private List<IComponent> _overrides;

    public List<IComponent> Overrides => _overrides;

    public void CreateEntity(Contexts contexts)
    {
        var gameEntity = contexts.game.CreateEntity();
        
        if (_template != null)
        {
            foreach (var component in _template.Components)
            {
                var componentType = component.GetType();
                if (_overrides == null || !_overrides.Any(c => c.GetType() == componentType))
                {
                    var index = Array.IndexOf(GameComponentsLookup.componentTypes, componentType);
                    gameEntity.AddComponent(index, component);
                }
            }
        }

        if (_overrides != null)
        {
            foreach (var component in _overrides)
            {
                var index = Array.IndexOf(GameComponentsLookup.componentTypes, component.GetType());
                gameEntity.AddComponent(index, component);
            }
        }
        

        gameObject.Link(gameEntity);
    }
}