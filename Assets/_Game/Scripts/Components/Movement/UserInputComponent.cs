using Entitas;
using UnityEngine;

[Input]
public class UserInputComponent : IComponent
{
    public Vector2 PreviousPosition;
    public Vector2 Axis;
    public bool IsFirePressed;
}