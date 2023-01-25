using Entitas;
using UnityEngine;

[Game]
public class AttackerComponent : IComponent
{
    public TargetType TargetType;
    public LayerMask TargetMask;
}