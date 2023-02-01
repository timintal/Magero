using UnityEngine;
using Entitas;

[Game]
public class WindImpulseComponent : IComponent
{
    public Vector3 Direction; 
    public float Power;
}