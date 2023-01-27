using Entitas;
using UnityEngine;

[Game]
public class CameraComponent : IComponent
{
    public Quaternion InitialForwardOffset;
    public Vector2 HorizontalRange;
    public Vector2 VerticalRange;
    public float RotationSpeed;
}
