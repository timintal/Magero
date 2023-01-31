using Entitas;
using UnityEngine;
using UnityEngine.Pool;

public class PoolComponent : IComponent
{
    public IObjectPool<GameObject> ParentPool;
}
[Game]
public class ResourceComponent : IComponent
{
    public GameObject Prefab;
}