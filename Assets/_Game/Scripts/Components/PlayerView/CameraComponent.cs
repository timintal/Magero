using Entitas;
using UnityEngine;

[Game]
public class CameraComponent : IComponent
{
    public Transform CameraTransform;
    public Vector2 HorizontalRange;
    public Vector2 VerticalRange;
    public float RotationSpeed;
}
