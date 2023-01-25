using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game, Unique]
public class ColliderCacheComponent : IComponent
{
    public Dictionary<Collider, int> ColliderCacheMap;
}