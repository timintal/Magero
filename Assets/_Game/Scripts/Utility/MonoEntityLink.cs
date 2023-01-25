using System;
using Entitas;
using Entitas.Unity;
using Sirenix.OdinInspector;
using UnityEngine;

public class MonoEntityLink : SerializedMonoBehaviour
{
    [SerializeField] private IComponent[] _components;

    public void CreateEntity(Contexts contexts)
    {
        var gameEntity = contexts.game.CreateEntity();
        foreach (var component in _components)
        {
            var index = Array.IndexOf(GameComponentsLookup.componentTypes, component.GetType());
            gameEntity.AddComponent(index, component);
        }   
        gameObject.Link(gameEntity);
    }
}
