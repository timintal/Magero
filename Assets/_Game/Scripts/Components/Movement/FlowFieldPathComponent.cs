using Entitas;
using UnityEngine;

[Game]
public class FlowFieldPathComponent : IComponent
{
    public Vector3[] path;
    public int stepsLeft;
}