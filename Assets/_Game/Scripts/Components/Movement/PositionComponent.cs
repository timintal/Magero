using System;
using Entitas;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable,Game]
public class PositionComponent : IComponent
{
    public Vector3 Value;

#if UNITY_EDITOR
    [Button(Expanded = true)]
    void SetFromTransform(Transform t)
    {
        Value = t.position;
    }
#endif
}